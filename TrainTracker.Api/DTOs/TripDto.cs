namespace TrainTracker.Api.DTOs;

public class TripDto
{
  public string TrainNumber { get; set; } = "";
  public string From { get; set; } = "";
  public string To { get; set; } = "";
  public DateTime DepartureTime { get; set; }
  public int DelayMinutes { get; set; }
  public int Platform { get; set; }
  public string? Type { get; set; }
}
