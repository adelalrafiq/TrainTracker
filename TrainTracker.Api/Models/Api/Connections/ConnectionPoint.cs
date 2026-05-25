using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api.Connections;

public class ConnectionPoint
{
  [JsonPropertyName("station")]
  public string Station { get; set; } = string.Empty;
  public StationInfo StationInfo { get; set; } = new StationInfo();
  [JsonPropertyName("time")]
  public long Time { get; set; }

  [JsonPropertyName("delay")]
  public int Delay { get; set; }

  [JsonPropertyName("platform")]
  public string Platform { get; set; } = string.Empty;

  [JsonPropertyName("vehicle")]
  public string Vehicle { get; set; } = string.Empty;
}
