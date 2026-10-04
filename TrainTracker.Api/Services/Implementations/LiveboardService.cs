using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrainTracker.Api.Hubs;
using TrainTracker.Api.Mappings;
using TrainTracker.Api.Models.Api;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Services.Implementations;

public class LiveboardService : ILiveboardService
{
  private readonly HttpClient _httpClient;
  private readonly IMemoryCache _cache;
  private readonly IHubContext<LiveboardHub> _hub;

  public LiveboardService(HttpClient httpClient, IMemoryCache cache, IHubContext<LiveboardHub> hub)
  {
    _httpClient = httpClient;
    _cache = cache;
    _hub = hub;
  }

  private static double? ParseCoordinate(string? value)
  {
    return double.TryParse(
        value,
        System.Globalization.NumberStyles.Any,
        System.Globalization.CultureInfo.InvariantCulture,
        out var result)
        ? result
        : null;
  }

  public async Task<LiveboardDto> GetLiveboard(string station)
  {
    var cacheKey = $"liveboard:{station.ToLower()}";

    // cache
    if (_cache.TryGetValue(cacheKey, out LiveboardDto? cachedData) && cachedData != null)
      return cachedData;

    // fetch fresh data
    var freshData = await FetchFromApi(station);

    // send realtime update
    Console.WriteLine("⚡ SIGNALR SEND TRIGGERED");
    await _hub.Clients.All.SendAsync("LiveboardUpdated", freshData);

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
      PropertyNameCaseInsensitive = true,
      NumberHandling = JsonNumberHandling.AllowReadingFromString
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
        row.LocationX = result.LocationX;
        row.LocationY = result.LocationY;
        row.DisplayStatus = BuildDisplayStatus(row, result.CurrentStationStatus);
      }
    }));
  }

  private async Task<StopsResultDto> GetStops(string vehicleId, string currentStation)
  {
    var cacheKey = $"vehicle:{vehicleId}:{currentStation.ToLowerInvariant()}";

    //  cache
    if (_cache.TryGetValue(cacheKey, out StopsResultDto? cached) && cached != null)
      return cached;

    try
    {
      var url = $"https://api.irail.be/vehicle/?id={Uri.EscapeDataString(vehicleId)}&format=json&lang=nl";

      var response = await _httpClient.GetStringAsync(url);

      var data = JsonSerializer.Deserialize<VehicleResponse>(response,
        new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true,
          NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
      Console.WriteLine(
    $"🔎 DESERIALIZED {vehicleId}: " +
    $"data={(data != null)}, " +
    $"stops={data?.Stops?.Stop?.Count ?? -1}, " +
    $"locationX={data?.VehicleInfo?.LocationX}, " +
    $"locationY={data?.VehicleInfo?.LocationY}");

      if (data?.Stops?.Stop == null)
      {
        Console.WriteLine(
            $"⚠️ No stops returned for vehicle {vehicleId}");
        return new StopsResultDto();
      }

      var allStops = data.Stops.Stop.ToList();

      Console.WriteLine(
          $"🚆 {vehicleId}: {allStops.Count} stops returned");

      foreach (var stop in allStops)
      {
        Console.WriteLine(
            $"   {stop.Station} | time={stop.Time} | " +
            $"arrived={stop.Arrived} | left={stop.Left} | " +
            $"delay={stop.Delay}");
      }

      // Find current station
      var currentIndex = allStops.FindIndex(s =>
          string.Equals(
              s.Station?.Trim(),
              currentStation.Trim(),
              StringComparison.OrdinalIgnoreCase
          )
      );

      if (currentIndex < 0)
      {
        Console.WriteLine(
            $"⚠️ Current station '{currentStation}' " +
            $"not found in vehicle stops.");

        return new StopsResultDto
        {
          LocationX = ParseCoordinate(data.VehicleInfo?.LocationX),
          LocationY = ParseCoordinate(data.VehicleInfo?.LocationY)
        };
      }
      var currentStop = allStops[currentIndex];

      // -----------------------------------------
      // Current station status
      // -----------------------------------------
      string currentStatus = "";
      if (currentStop.Canceled == "1")
      {
        currentStatus = "geannuleerd";
      }
      else if (currentStop.Arrived == "1"
               && currentStop.Left == "0")
      {
        currentStatus = "aan perron";
      }
      else if (currentStop.Arrived == "0"
               && currentStop.Left == "0")
      {
        if (long.TryParse(
                currentStop.ScheduledArrivalTime,
                out var scheduledArrival)
            && int.TryParse(
                currentStop.ArrivalDelay,
                out var arrivalDelay))
        {
          var arrivalTime =
              DateTimeOffset
                  .FromUnixTimeSeconds(scheduledArrival)
                  .AddSeconds(arrivalDelay);

          var secondsUntilArrival =
              (arrivalTime - DateTimeOffset.UtcNow).TotalSeconds;

          if (secondsUntilArrival > 0
              && secondsUntilArrival <= 90)
          {
            currentStatus = "komt aan";
          }
        }
      }

      // -----------------------------------------
      // Upcoming stops
      // -----------------------------------------     
      var upcomingStops = allStops
           .Skip(currentIndex + 1)
           .Take(4)
           .Select(stop =>
           {
             DateTimeOffset? arrivalTime = null;

             if (long.TryParse(
                      stop.ScheduledArrivalTime,
                      out var scheduledArrival))
             {
               var arrivalDelay =
                    int.TryParse(
                        stop.ArrivalDelay,
                        out var delay)
                        ? delay
                        : 0;

               arrivalTime =
                    DateTimeOffset
                        .FromUnixTimeSeconds(scheduledArrival)
                        .AddSeconds(arrivalDelay);
             }

             return new StopDto
             {
               Station = stop.Station,
               ArrivalTime = arrivalTime,
               Status = ""
             };
           })
           .ToList();

      var result = new StopsResultDto
      {
        Stops = upcomingStops,
        CurrentStationStatus = currentStatus,
        LocationX = ParseCoordinate(data.VehicleInfo?.LocationX),
        LocationY = ParseCoordinate(data.VehicleInfo?.LocationY)
      };

      _cache.Set(
          cacheKey,
          result,
          TimeSpan.FromSeconds(10));

      return result;
    }
    catch (Exception ex)
    {
      Console.WriteLine(
          $"❌ GetStops failed for {vehicleId}: {ex}");

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