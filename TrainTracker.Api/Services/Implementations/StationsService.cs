using System.Text.Json;
using TrainTracker.Api.Models.Api;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Services.Implementations;

public class StationsService : IStationsService
{
  private readonly HttpClient _httpClient;
  private static List<string> _cachedStations = new();

  public StationsService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }
  public async Task<List<StationDto>> SearchStations(string query)
  {
    if (string.IsNullOrWhiteSpace(query))
      return new List<StationDto>();

    // load once
    if (!_cachedStations.Any())
    {
      var url = "https://api.irail.be/stations/?format=json&lang=nl";
      var response = await _httpClient.GetStringAsync(url);
      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      var data = JsonSerializer.Deserialize<StationsResponse>(response, options);
      _cachedStations = data?.Stations?
        .Select(s => s.Name)
        .Where(n => !string.IsNullOrEmpty(n))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList()
        ?? new List<string>();
    }

    // filter    
    return _cachedStations
      .Where(s =>
      s.StartsWith(query, StringComparison.OrdinalIgnoreCase))
      .Select(s => new StationDto { Name = s })
      .ToList();
  }
}
