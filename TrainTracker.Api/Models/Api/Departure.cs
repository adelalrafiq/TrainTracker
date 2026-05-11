namespace TrainTracker.Api.Models.Api;

public class Departure
{
  public string Station { get; set; } = string.Empty;
  public string Time { get; set; } = string.Empty;
  public string Delay { get; set; } = string.Empty;

  public string Platform { get; set; } = string.Empty;

  public string Canceled { get; set; } = string.Empty;
  public string Vehicle { get; set; } = string.Empty;
  public VehicleInfo VehicleInfo { get; set; } = new();
}
