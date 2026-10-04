using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api;

public class VehicleResponse
{
  [JsonPropertyName("vehicleinfo")]
  public VehicleInfo VehicleInfo { get; set; } = new();

  [JsonPropertyName("stops")]
  public VehicleStops Stops { get; set; } = new();
}
