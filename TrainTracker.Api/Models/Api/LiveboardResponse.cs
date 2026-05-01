namespace TrainTracker.Api.Models.Api;

public class LiveboardResponse
{
  public string Station { get; set; }
  public StationInfo StationInfo { get; set; }
  public DeparturesWrapper Departures { get; set; }
}
