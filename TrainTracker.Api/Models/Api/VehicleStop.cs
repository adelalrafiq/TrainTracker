using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api;

public class VehicleStop
{
  [JsonPropertyName("id")]
  public string Id { get; set; } = string.Empty;

  [JsonPropertyName("station")]
  public string Station { get; set; } = string.Empty;

  [JsonPropertyName("time")]
  public string Time { get; set; } = string.Empty;

  [JsonPropertyName("scheduledDepartureTime")]
  public string ScheduledDepartureTime { get; set; } = string.Empty;

  [JsonPropertyName("scheduledArrivalTime")]
  public string ScheduledArrivalTime { get; set; } = string.Empty;

  [JsonPropertyName("delay")]
  public string Delay { get; set; } = string.Empty;

  [JsonPropertyName("arrivalDelay")]
  public string ArrivalDelay { get; set; } = string.Empty;

  [JsonPropertyName("departureDelay")]
  public string DepartureDelay { get; set; } = string.Empty;

  [JsonPropertyName("canceled")]
  public string Canceled { get; set; } = string.Empty;

  [JsonPropertyName("arrivalCanceled")]
  public string ArrivalCanceled { get; set; } = string.Empty;

  [JsonPropertyName("departureCanceled")]
  public string DepartureCanceled { get; set; } = string.Empty;

  [JsonPropertyName("left")]
  public string Left { get; set; } = string.Empty;

  [JsonPropertyName("arrived")]
  public string Arrived { get; set; } = string.Empty;
}
