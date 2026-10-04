using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api;

public class VehicleStops
{
  [JsonPropertyName("number")]
  public string Number { get; set; } = string.Empty;

  [JsonPropertyName("stop")]
  public List<VehicleStop> Stop { get; set; } = [];
}
