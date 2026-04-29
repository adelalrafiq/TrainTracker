namespace TrainTracker.Api.Models;

public class DelayRecord
{
  public int Id { get; set; }

  public int TripId { get; set; }
  public Trip? Trip { get; set; }

  public int DelayMinutes { get; set; }

  public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
