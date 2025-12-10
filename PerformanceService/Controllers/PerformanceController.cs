using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerformanceService.DTOs;
using PerformanceService.Services;

namespace PerformanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerformanceController : ControllerBase
{
    private readonly ICompetitionSimulator _simulator;
    private readonly ILeaderboardNotifier _notifier;
    private readonly ILogger<PerformanceController> _logger;

    public PerformanceController(ICompetitionSimulator simulator, ILeaderboardNotifier notifier, ILogger<PerformanceController> logger)
    {
        _simulator = simulator;
        _notifier = notifier;
        _logger = logger;
    }

    // POST api/performance/simulate
    [HttpPost("simulate")]
    public async Task<IActionResult> Simulate([FromBody] SimulateRequestDto dto)
    {
        try
        {
            var updated = await _simulator.SimulateAndPersistAsync(dto.PlayerIds, dto.Games);

            // Map to lightweight objects for notification
            var notification = updated.Select(u => new
            {
                PlayerId = u.PlayerId,
                u.GamesPlayed,
                u.Points,
                u.Assists,
                u.Rebounds,
                u.LastUpdated
            });

            // Notify leaderboard asynchronously; failures are logged inside notifier
            await _notifier.NotifyAsync(notification);

            return Ok(new { Updated = notification.Count(), Values = notification });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Simulation failed");
            return StatusCode(500, "Simulation failed");
        }
    }

    // GET api/performance/{playerId}
    [HttpGet("{playerId}")]
    public async Task<IActionResult> Get(int playerId, [FromServices] PerformanceService.Data.PerformanceDbContext db)
    {
        var stat = await db.PerformanceStats.FirstOrDefaultAsync(s => s.PlayerId == playerId);
        if (stat == null) return NotFound();
        return Ok(stat);
    }
}