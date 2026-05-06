namespace TrainTracker.Api.Models.DTOs;

public class LiveboardRowDto
{
  /// <example>Antwerp-Central</example>
  public string DirectionName { get; set; }
  /// <example>2026-05-04T08:30:00</example>
  public DateTimeOffset DepartureTime { get; set; }
  /// <example>4</example>
  public string Platform { get; set; }
  /// <example>IC 1830</example>
  public string VehicleInfoShortname { get; set; }
  /// <example>5</example>
  public int DelayMinutes { get; set; }
  public TrainStatus Status { get; set; }
  public string VehicleId { get; set; }
  public List<string> Stops { get; set; } = new List<string>();
}
