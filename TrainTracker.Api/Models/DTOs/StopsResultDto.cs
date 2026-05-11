namespace TrainTracker.Api.Models.DTOs;

public class StopsResultDto
{
  public List<StopDto> Stops { get; set; } = [];

  public string CurrentStationStatus { get; set; } = "";
}
