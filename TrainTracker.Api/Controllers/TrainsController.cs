using Microsoft.AspNetCore.Mvc;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainsController : ControllerBase
{
  private readonly ITrainApiService _service;

  public TrainsController(ITrainApiService service)
  {
    _service = service;
  }
  [HttpGet("live")]
  public async Task<IActionResult> GetLive(string station = "Antwerpen-Centraal")
  {
    var data = await _service.GetLiveBoardAsync(station);
    return Ok(data);
  }
}
