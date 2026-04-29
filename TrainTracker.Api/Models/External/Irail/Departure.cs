namespace TrainTracker.Api.Models.External.Irail;

public class Departure
{
  public string vehicle { get; set; } = "";
  public string station { get; set; } = "";
  public string time { get; set; } = "";
  public string delay { get; set; } = "";
  public string? platform { get; set; }
}
