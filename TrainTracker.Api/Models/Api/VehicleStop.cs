namespace TrainTracker.Api.Models.Api;

public class VehicleStop
{
  public string Station { get; set; } = string.Empty;
  public string Time { get; set; } = string.Empty;
  public string Delay { get; set; } = string.Empty;
  public string Left { get; set; } = string.Empty;
  public string Arrived { get; set; } = string.Empty;
}
