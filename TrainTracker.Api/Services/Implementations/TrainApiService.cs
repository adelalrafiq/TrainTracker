using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TrainTracker.Api.Data;
using TrainTracker.Api.DTOs;
using TrainTracker.Api.Hubs;
using TrainTracker.Api.Models;
using TrainTracker.Api.Models.External.Irail;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Services.Implementations;

public class TrainApiService : ITrainApiService
{
  private readonly HttpClient _httpClient;
  private readonly ApplicationDbContext _context;
  private readonly IMemoryCache _cache;
  private readonly IHubContext<TrainHub> _hubContext;
  public TrainApiService(
    HttpClient httpClient,
    ApplicationDbContext context,
    IMemoryCache cache,
    IHubContext<TrainHub> hubContext)
  {
    _httpClient = httpClient;
    _context = context;
    _cache = cache;
    _hubContext = hubContext;
  }

  //===================
  // Main Method
  //===================
  public async Task<List<TripDto>> GetLiveBoardAsync(string station)
  {
    string cacheKey = $"trains_{station}";

    // Cache
    if (_cache.TryGetValue(cacheKey, out List<TripDto>? cachedData))
    {
      return cachedData ?? new List<TripDto>();
    }
    try
    {
      // External API
      var url = $"https://api.irail.be/liveboard/?station={station}&format=json";
      var response = await _httpClient.GetAsync(url);

      if (!response.IsSuccessStatusCode)
        return new List<TripDto>();

      var json = await response.Content.ReadAsStringAsync();
      var result = JsonSerializer.Deserialize<IrailResponse>(json);
      var trips = result?.departures?.departure?.Select(d => new TripDto
      {
        TrainNumber = d.vehicle,
        From = station,
        To = d.station,
        DepartureTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(d.time)).DateTime,
        DelayMinutes = int.Parse(d.delay) / 60,
        Type = GetTrainType(d.vehicle),
        Platform = int.TryParse(d.platform ?? "0", out var p) ? p : 0
      }).ToList() ?? new List<TripDto>();

      // Save to DB
      await SaveTripsToDatabase(trips);

      // Cache (5 min)
      _cache.Set(cacheKey, trips, TimeSpan.FromMinutes(5));

      // SignalR (live update)
      await _hubContext.Clients.All.SendAsync("ReceiveTrains", trips);

      return trips;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error: {ex.Message}");
      return new List<TripDto>();
    }
  }

  //========================
  // Save to Database (Optimized)
  //========================
  private async Task SaveTripsToDatabase(List<TripDto> trips)
  {
    var trainCache = new Dictionary<string, Train>();
    var stationCache = new Dictionary<string, Station>();
    foreach (var trip in trips)
    {
      // ===== Train =====
      if (!trainCache.TryGetValue(trip.TrainNumber, out var train))
      {
        train = await _context.Trains
            .FirstOrDefaultAsync(t => t.TrainNumber == trip.TrainNumber);

        if (train == null)
        {
          train = new Train
          {
            TrainNumber = trip.TrainNumber,
            Type = trip.Type ?? "Unknown"
          };

          _context.Trains.Add(train);
        }

        trainCache[trip.TrainNumber] = train;
      }

      // ===== Stations =====
      var departureStation = await GetOrCreateStation(trip.From, stationCache);
      var arrivalStation = await GetOrCreateStation(trip.To, stationCache);

      // ❗ Prevent duplicates
      var exists = await _context.Trips.AnyAsync(t =>
          t.TrainId == train.Id &&
          t.DepartureTime == trip.DepartureTime);

      if (exists)
        continue;

      // ===== Create Trip =====
      var tripEntity = new Trip
      {
        Train = train,
        DepartureStation = departureStation,
        ArrivalStation = arrivalStation,
        DepartureTime = trip.DepartureTime,
        ArrivalTime = trip.DepartureTime.AddHours(1),
        Platform = trip.Platform,
        Status = trip.DelayMinutes > 0 ? "Delayed" : "OnTime"
      };

      _context.Trips.Add(tripEntity);

      // SignalR: Train Updated
      await _hubContext.Clients.All.SendAsync("TrainUpdated", new
      {
        trip.TrainNumber,
        trip.From,
        trip.To,
        trip.DepartureTime,
        trip.Platform,
        Status = trip.DelayMinutes > 0 ? "Delayed" : "OnTime"
      });

      // ===== Delay Tracking =====
      if (trip.DelayMinutes > 0)
      {
        _context.DelayRecords.Add(new DelayRecord
        {
          Trip = tripEntity,
          DelayMinutes = trip.DelayMinutes
        });

        // SignalR: only delayed update
        await _hubContext.Clients.All.SendAsync("TrainDelayed", new
        {
          trip.TrainNumber,
          trip.DelayMinutes
        });
      }
    }

    // Single SaveChanges (performance)
    await _context.SaveChangesAsync();
  }

  //=============================
  // GET or Create Station
  //=============================
  private async Task<Station> GetOrCreateStation(string name,
     Dictionary<string, Station> stationCache)
  {
    if (stationCache.TryGetValue(name, out var cachedStation))
      return cachedStation;

    var station = await _context.Stations
        .FirstOrDefaultAsync(s => s.Name == name);

    if (station == null)
    {
      station = new Station
      {
        Name = name,
        City = name
      };

      _context.Stations.Add(station);
    }
    stationCache[name] = station;
    return station;
  }


  // =========================================
  // TRAIN TYPE
  // =========================================
  private string GetTrainType(string vehicle)
  {
    if (vehicle.Contains("IC")) return "IC";
    if (vehicle.Contains("S")) return "S";
    if (vehicle.Contains("L")) return "L";

    return "Unknown";
  }
}
