namespace TrainTracker.Api.Models.DTOs;

public class StopsResultDto
{
  public List<StopDto> Stops { get; set; } = [];

  public string CurrentStationStatus { get; set; } = string.Empty;
  public double? LocationX { get; set; }
  public double? LocationY { get; set; }
}
