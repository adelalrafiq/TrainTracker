namespace TrainTracker.Api.Models.Api;

public class StationInfo
{
  public double LocationX { get; set; } // Longitude
  public double LocationY { get; set; } // Latitude
  public string Name { get; set; } = string.Empty;
}
