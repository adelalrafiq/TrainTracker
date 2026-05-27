using System.ComponentModel.DataAnnotations;

namespace TrainTracker.Api.Models.DTOs;

public class LiveboardDto
{
  [Required]
  public string StationName { get; set; } = string.Empty;
  public double Longitude { get; set; }
  public double Latitude { get; set; }
  public List<LiveboardRowDto> Rows { get; set; } = [];
}
