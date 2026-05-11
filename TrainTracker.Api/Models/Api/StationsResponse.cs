using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api;

public class StationsResponse
{
  [JsonPropertyName("station")]
  public List<StationApi> Stations { get; set; } = [];
}
