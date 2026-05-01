namespace TrainTracker.Api.Models.Api;

public class Departure
{
  public string Station { get; set; }
  public string Time { get; set; }
  public string Delay { get; set; }

  public string Platform { get; set; }

  public string Canceled { get; set; }

  public VehicleInfo VehicleInfo { get; set; }
}
