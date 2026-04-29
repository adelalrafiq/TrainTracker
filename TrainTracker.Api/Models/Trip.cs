namespace TrainTracker.Api.Models;

public class Trip
{
  public int Id { get; set; }

  public int TrainId { get; set; }
  public Train? Train { get; set; }

  public int DepartureStationId { get; set; }
  public Station? DepartureStation { get; set; }

  public int ArrivalStationId { get; set; }
  public Station? ArrivalStation { get; set; }

  public DateTime DepartureTime { get; set; }

  public DateTime ArrivalTime { get; set; }

  public int Platform { get; set; }

  public string Status { get; set; } = "OnTime"; // OnTime / Delayed

  // Navigation 
  public ICollection<DelayRecord>? Delays { get; set; }
}
