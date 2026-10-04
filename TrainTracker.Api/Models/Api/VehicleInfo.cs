using System.Text.Json.Serialization;

namespace TrainTracker.Api.Models.Api;

public class VehicleInfo
{
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;

  [JsonPropertyName("shortname")]
  public string ShortName { get; set; } = string.Empty;

  [JsonPropertyName("number")]
  public string Number { get; set; } = string.Empty;

  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;

  [JsonPropertyName("locationX")]
  public string? LocationX { get; set; }

  [JsonPropertyName("locationY")]
  public string? LocationY { get; set; }

  [JsonPropertyName("@id")]
  public string? Id { get; set; }
}
