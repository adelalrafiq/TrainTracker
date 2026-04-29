namespace TrainTracker.Api.Models;

public class Station
{
  public int Id { get; set; }

  public string Name { get; set; } = string.Empty;

  public string City { get; set; } = string.Empty;

  public string Country { get; set; } = "Belgium";
  // Navigation
  public ICollection<Trip>? DepartureTrips { get; set; }
  public ICollection<Trip>? ArrivalTrips { get; set; }
}
