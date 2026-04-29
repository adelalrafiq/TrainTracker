using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainTracker.Api.Data;

namespace TrainTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
  private readonly ApplicationDbContext _context;

  public AnalyticsController(ApplicationDbContext context)
  {
    _context = context;
  }
  [HttpGet("status")]
  public async Task<IActionResult> GetStatusStats()
  {
    var total = await _context.Trips.CountAsync();

    var delayed = await _context.Trips
        .CountAsync(t => t.Status == "Delayed");

    var onTime = total - delayed;

    return Ok(new
    {
      labels = new[] { "On Time", "Delayed" },
      data = new[] { onTime, delayed }
    });
  }

  [HttpGet("delays-over-time")]
  public async Task<IActionResult> GetDelaysOverTime()
  {
    var data = await _context.DelayRecords
        .GroupBy(d => d.RecordedAt.Date)
        .Select(g => new
        {
          date = g.Key,
          totalDelay = g.Sum(x => x.DelayMinutes)
        })
        .OrderBy(x => x.date)
        .ToListAsync();

    return Ok(new
    {
      labels = data.Select(x => x.date.ToString("yyyy-MM-dd")),
      data = data.Select(x => x.totalDelay)
    });
  }

  [HttpGet("average-delay")]
  public async Task<IActionResult> GetAverageDelay()
  {
    var avg = await _context.DelayRecords
        .AverageAsync(d => (double?)d.DelayMinutes) ?? 0;

    return Ok(new
    {
      averageDelay = Math.Round(avg, 2)
    });
  }

  [HttpGet("top-delayed-stations")]
  public async Task<IActionResult> GetTopDelayedStations()
  {
    var data = await _context.Trips
        .Where(t => t.Status == "Delayed")
        .GroupBy(t => t.DepartureStation!.Name)
        .Select(g => new
        {
          station = g.Key,
          count = g.Count()
        })
        .OrderByDescending(x => x.count)
        .Take(5)
        .ToListAsync();

    return Ok(new
    {
      labels = data.Select(x => x.station),
      data = data.Select(x => x.count)
    });
  }
}
