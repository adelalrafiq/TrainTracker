using Microsoft.AspNetCore.Mvc;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;


namespace TrainTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LiveboardController : ControllerBase
{
  private readonly ILiveboardService _liveboardService;

  public LiveboardController(ILiveboardService liveboardService)
  {
    _liveboardService = liveboardService;
  }

  // GET: api/liveboard/station=Gent
  /// <summary>
  /// Get live departure board for a station
  /// </summary>
  /// <param name="station">Station name (e.g. Sint-Niklaas)</param>
  /// <returns>List of upcoming departures</returns>
  [HttpGet("{station}")]
  [ProducesResponseType(typeof(LiveboardDto), 200)]
  [ProducesResponseType(400)]
  public async Task<IActionResult> GetLiveboard(string station)
  {
    if (string.IsNullOrWhiteSpace(station))
      return BadRequest("Station is required");

    var result = await _liveboardService.GetLiveboard(station);

    return Ok(result);
  }
}