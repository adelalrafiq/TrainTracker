using Microsoft.AspNetCore.Mvc;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationsController : ControllerBase
{
  private readonly IStationsService _stationsService;

  public StationsController(IStationsService stationsService)
  {
    _stationsService = stationsService;
  }
  [HttpGet]
  public async Task<IActionResult> Search([FromQuery] string query)
  {
    var result = await _stationsService.SearchStations(query);
    return Ok(result);
  }
}
