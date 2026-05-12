using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TrainTracker.Api.Mappings;
using TrainTracker.Api.Models.Api;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Services.Implementations;

public class LiveboardService : ILiveboardService
{
  private readonly HttpClient _httpClient;
  private readonly IMemoryCache _cache;

  public LiveboardService(HttpClient httpClient, IMemoryCache cache)
  {
    _httpClient = httpClient;
    _cache = cache;
  }
  public async Task<LiveboardDto> GetLiveboard(string station)
  {
    var cacheKey = $"liveboard:{station.ToLower()}";

    // cache
    if (_cache.TryGetValue(cacheKey, out LiveboardDto? cachedData) && cachedData != null)
      return cachedData;

    // fetch fresh data
    var freshData = await FetchFromApi(station);

    // cache options
    var cacheOptions = new MemoryCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30), // time to live
      SlidingExpiration = TimeSpan.FromSeconds(10) // refreshes if accessed
    };

    _cache.Set(cacheKey, freshData, cacheOptions);

    return freshData;
  }

  private async Task<LiveboardDto> FetchFromApi(string station)
  {
    var url = $"https://api.irail.be/liveboard/?station={station}&arrdep=departure&format=json&lang=nl";

    var response = await _httpClient.GetStringAsync(url);

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var data = JsonSerializer.Deserialize<LiveboardResponse>(response, options);

    if (data == null)
      return new LiveboardDto { Rows = new List<LiveboardRowDto>() };

    var dto = data.ToDto(); // mapping from API model to DTO
    await AddStopsToRows(dto.Rows, station); // Add stops info to each row
    return dto;
  }

  private async Task AddStopsToRows(List<LiveboardRowDto> rows, string currentStation)
  {
    await Task.WhenAll(rows.Select(async row =>
    {
      if (!string.IsNullOrEmpty(row.VehicleId))
      {
        var result = await GetStops(row.VehicleId, currentStation);
        row.Stops = result.Stops;
        row.DisplayStatus = BuildDisplayStatus(row, result.CurrentStationStatus);
      }
    }));
  }

  private async Task<StopsResultDto> GetStops(string vehicleId, string currentStation)
  {
    var cacheKey = $"vehicle:{vehicleId}:{currentStation.ToLower()}";

    //  cache
    if (_cache.TryGetValue(cacheKey, out StopsResultDto? cached) && cached != null)
      return cached;

    try
    {
      var url = $"https://api.irail.be/vehicle/?id={Uri.EscapeDataString(vehicleId)}&format=json&lang=nl";

      var response = await _httpClient.GetStringAsync(url);

      var data = JsonSerializer.Deserialize<VehicleResponse>(response,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

      var allStops =
       data?.Stops?.Stop?.ToList()
       ?? new List<VehicleStop>();

      if (!allStops.Any())
      {
        return new StopsResultDto();
      }

      var now = DateTimeOffset.UtcNow;

      // current station index
      var currentIndex = allStops.FindIndex(s =>
          s.Station.Equals(
            currentStation,
            StringComparison.OrdinalIgnoreCase));

      if (currentIndex < 0)
      {
        return new StopsResultDto();
      }

      var currentStop = allStops[currentIndex];

      // Current station status
      string currentStatus = "";

      // scheduled arrival time
      var scheduledTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(currentStop.Time));

      // iRail delay is seconds 
      var delaySeconds = int.TryParse(currentStop.Delay, out var d) ? d : 0;

      // real arrival time
      var eta = scheduledTime.AddSeconds(delaySeconds);

      // seconds until arrival
      var secondsToArrival = (eta - now).TotalSeconds;

      if (currentStop.Arrived == "1"
      && currentStop.Left == "0")
      {
        currentStatus = "aan perron";
      }
      // train arriving very soon
      else if (currentStop.Arrived == "0"
        && secondsToArrival <= 90
         && secondsToArrival > 0)
      {
        currentStatus = "komt aan";
      }

      // Upcoming Stops (After current station)      
      var finalDestination = allStops.LastOrDefault()?.Station;
      var upcomingStops = allStops
          .Skip(currentIndex + 1)
          // remove destination station
          .Where(s => !s.Station.Equals(
                  finalDestination,
                  StringComparison.OrdinalIgnoreCase))
          .Take(3)
          .Select(s =>
          {
            var stopTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(s.Time));
            return new StopDto
            {
              Station = s.Station,
              ArrivalTime = stopTime,
              Status = ""
            };
          })
          .ToList()
          ?? new List<StopDto>();

      var result = new StopsResultDto
      {
        Stops = upcomingStops,
        CurrentStationStatus = currentStatus
      };

      _cache.Set(cacheKey, result, TimeSpan.FromSeconds(10));

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return new StopsResultDto();
    }
  }

  private string BuildDisplayStatus(LiveboardRowDto row, string currentStationStatus)
  {
    // geannuleerd
    if (row.Status == TrainStatus.Canceled)
    {
      return "geannuleerd";
    }
    // current station status
    if (!string.IsNullOrEmpty(currentStationStatus))
    {
      return currentStationStatus;
    }
    // default
    return "";
  }
}