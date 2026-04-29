namespace TrainTracker.Api.Models;

public class Train
{
  public int Id { get; set; }
  public string TrainNumber { get; set; } = string.Empty; // IC123
  public string Type { get; set; } = string.Empty; // IC, S, etc.
  // Navigation
  public ICollection<Trip>? Trips { get; set; }
}
