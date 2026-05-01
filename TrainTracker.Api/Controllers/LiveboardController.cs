using Microsoft.AspNetCore.Mvc;
using TrainTracker.Api.Services.Interfaces;


namespace TrainTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LiveboardController : ControllerBase
{
  private readonly ILiveboardService _liveboardService;

  public LiveboardController(ILiveboardService liveboardService)
  {
    _liveboardService = liveboardService;
  }

  // GET: api/liveboard?station=Ghent
  [HttpGet("{station}")]
  public async Task<IActionResult> GetLiveboard(string station)
  {
    if (string.IsNullOrWhiteSpace(station))
      return BadRequest("Station is required");

    var result = await _liveboardService.GetLiveboard(station);

    return Ok(result);
  }
}