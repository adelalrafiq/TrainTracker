using System.Text.Json;
using System.Text.Json.Serialization;
using TrainTracker.Api.Models.Api.Connections;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Services.Implementations;

public class ConnectionsService : IConnectionsService
{
  private readonly HttpClient _httpClient;
  public ConnectionsService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<List<ConnectionDto>> GetConnectionsAsync(
            string from,
            string to,
            string? date = null,
            string? time = null)
  {
    var now = DateTime.Now;
    date ??= now.ToString("ddMMyy");
    time ??= now.ToString("HHmm");
    var url =
                  $"https://api.irail.be/connections/?" +
                  $"from={Uri.EscapeDataString(from)}" +
                  $"&to={Uri.EscapeDataString(to)}" +
                  $"&date={date}" +
                  $"&time={time}" +
                  $"&timesel=departure" +
                  $"&format=json" +
                  $"&lang=nl" +
                  $"&typeOfTransport=automatic" +
                  $"&alerts=false" +
                  $"&results=6";

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("Accept", "application/json");
    var response = await _httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();
    var json = await response.Content.ReadAsStringAsync();

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true,
      NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    var apiResponse = JsonSerializer.Deserialize<ConnectionsResponse>(
              json,
              options);

    if (apiResponse == null || apiResponse.Connection == null)
    {
      return new List<ConnectionDto>();
    }

    return apiResponse.Connection.Select(c => new ConnectionDto
    {
      Id = c.Id,

      DepartureStation = c.Departure.Station,
      DepartureLocation = new StationLocationDto
      {
        Lat = c.Departure.StationInfo.LocationY,
        Lng = c.Departure.StationInfo.LocationX
      },
      ArrivalStation = c.Arrival.Station,
      ArrivalLocation = new StationLocationDto
      {
        Lat = c.Arrival.StationInfo.LocationY,
        Lng = c.Arrival.StationInfo.LocationX
      },

      DepartureTime = DateTimeOffset
                       .FromUnixTimeSeconds(c.Departure.Time)
                       .DateTime,

      ArrivalTime = DateTimeOffset
                       .FromUnixTimeSeconds(c.Arrival.Time)
                       .DateTime,

      DepartureDelay = c.Departure.Delay,
      ArrivalDelay = c.Arrival.Delay,

      Duration = c.Duration,
      Vehicle = c.Departure.Vehicle,
      DeparturePlatform = c.Departure.Platform,
      ArrivalPlatform = c.Arrival.Platform
    }).ToList();
  }
}
