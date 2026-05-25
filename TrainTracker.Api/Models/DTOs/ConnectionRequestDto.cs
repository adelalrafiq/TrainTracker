using System.ComponentModel.DataAnnotations;

namespace TrainTracker.Api.Models.DTOs;

public class ConnectionRequestDto
{
  [Required]
  public string From { get; set; } = string.Empty;

  [Required]
  public string To { get; set; } = string.Empty;

  public string? Date { get; set; }

  public string? Time { get; set; }
}
