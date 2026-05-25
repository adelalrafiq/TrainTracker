using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api.Connections;

public class ConnectionsResponse
{
  [JsonPropertyName("version")]
  public string Version { get; set; } = string.Empty;

  [JsonPropertyName("timestamp")]
  [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
  public long Timestamp { get; set; }

  [JsonPropertyName("connection")]
  public List<Connection> Connection { get; set; } = new();
}
