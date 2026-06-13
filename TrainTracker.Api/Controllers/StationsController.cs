using Microsoft.AspNetCore.Mvc;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StationsController : ControllerBase
{
  private readonly IStationsService _stationsService;

  public StationsController(IStationsService stationsService)
  {
    _stationsService = stationsService;
  }

  /// <summary>
  /// Search stations by name
  /// </summary>
  /// <param name="query">Search keyword (e.g. Gen)</param> 
  [HttpGet]
  [ProducesResponseType(typeof(List<StationDto>), 200)]
  public async Task<IActionResult> Search([FromQuery] string query)
  {
    var result = await _stationsService.SearchStations(query);
    return Ok(result);
  }
}
