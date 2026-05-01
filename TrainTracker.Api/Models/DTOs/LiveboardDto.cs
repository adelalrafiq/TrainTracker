namespace TrainTracker.Api.Models.DTOs;

public class LiveboardDto
{
  public string StationName { get; set; }
  public double Longitude { get; set; }
  public double Latitude { get; set; }
  public List<LiveboardRowDto> Rows { get; set; }
}
