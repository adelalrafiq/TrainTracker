namespace TrainTracker.Api.Models.Api;

public class LiveboardResponse
{
  public string Station { get; set; } = string.Empty;
  public StationInfo StationInfo { get; set; } = new();
  public DeparturesWrapper Departures { get; set; } = new();
}
