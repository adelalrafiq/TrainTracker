using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api;

public class StationApi
{
  [JsonPropertyName("name")]
  public string Name { get; set; }
}
