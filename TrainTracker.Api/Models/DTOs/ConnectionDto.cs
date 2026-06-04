using System.ComponentModel.DataAnnotations;

namespace TrainTracker.Api.Models.DTOs;

public class ConnectionDto
{
  public int Id { get; set; }

  [Required]
  public string DepartureStation { get; set; } = string.Empty;
  public StationLocationDto DepartureLocation { get; set; } = new();
  [Required]
  public string ArrivalStation { get; set; } = string.Empty;
  public StationLocationDto ArrivalLocation { get; set; } = new();
  public DateTimeOffset DepartureTime { get; set; }
  public DateTimeOffset ArrivalTime { get; set; }
  public int DepartureDelay { get; set; }
  public int ArrivalDelay { get; set; }
  public int Duration { get; set; }
  public string Vehicle { get; set; } = string.Empty;

  public string DeparturePlatform { get; set; } = string.Empty;
  public string ArrivalPlatform { get; set; } = string.Empty;
}
