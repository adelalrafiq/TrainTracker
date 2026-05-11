namespace TrainTracker.Api.Models.DTOs;

public class StopDto
{
  public string Station { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public DateTimeOffset ArrivalTime { get; set; }
}
