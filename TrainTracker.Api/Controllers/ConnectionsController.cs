using Microsoft.AspNetCore.Mvc;
using TrainTracker.Api.Models.DTOs;
using TrainTracker.Api.Services.Interfaces;

namespace TrainTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConnectionsController : ControllerBase
{
  private readonly IConnectionsService _connectionsService;
  public ConnectionsController(IConnectionsService connectionsService)
  {
    _connectionsService = connectionsService;
  }

  [HttpGet]
  public async Task<ActionResult<List<ConnectionDto>>> GetConnections(
    [FromQuery] ConnectionRequestDto request)
  {
    try
    {
      var result = await _connectionsService.GetConnectionsAsync(
        request.From,
        request.To,
        request.Date,
        request.Time);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }
}
