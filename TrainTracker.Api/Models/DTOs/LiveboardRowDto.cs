namespace TrainTracker.Api.Models.DTOs;

public class LiveboardRowDto
{
  public string DirectionName { get; set; }
  public DateTimeOffset DepartureTime { get; set; }
  public string Platform { get; set; }
  public string VehicleInfoShortname { get; set; }
  public int DelayMinutes { get; set; }
  public TrainStatus Status { get; set; }
}
