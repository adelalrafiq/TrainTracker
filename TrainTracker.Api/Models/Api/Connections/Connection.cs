using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api.Connections;

public class Connection
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("departure")]
  public ConnectionPoint Departure { get; set; } = new ConnectionPoint();

  [JsonPropertyName("arrival")]
  public ConnectionPoint Arrival { get; set; } = new ConnectionPoint();

  [JsonPropertyName("duration")]
  public int Duration { get; set; }
}
