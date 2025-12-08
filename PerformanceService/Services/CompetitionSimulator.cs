using Microsoft.EntityFrameworkCore;
using PerformanceService.Data;
using PerformanceService.Models;
using Microsoft.Extensions.Logging;

namespace PerformanceService.Services;

public class CompetitionSimulator : ICompetitionSimulator
{
    private readonly PerformanceDbContext _db;
    private readonly ILogger<CompetitionSimulator> _logger;

    public CompetitionSimulator(PerformanceDbContext db, ILogger<CompetitionSimulator> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<List<PerformanceStat>> SimulateAndPersistAsync(List<int>? playerIds, int games)
    {
        // Select players to simulate. If playerIds null, pick some from table.
        List<int> targets;
        if (playerIds == null || playerIds.Count == 0)
        {
            targets = await _db.PerformanceStats.Select(p => p.PlayerId).Take(10).ToListAsync();
        }
        else
        {
            targets = playerIds;
        }

        var updated = new List<PerformanceStat>();

        var rng = Random.Shared;

        foreach (var pid in targets)
        {
            var stat = await _db.PerformanceStats.FirstOrDefaultAsync(p => p.PlayerId == pid);
            if (stat == null)
            {
                stat = new PerformanceStat { PlayerId = pid };
                _db.PerformanceStats.Add(stat);
            }

            // Simple simulation: add per-game random stats
            for (int g = 0; g < games; g++)
            {
                stat.GamesPlayed += 1;
                stat.Points += rng.Next(0, 40);
                stat.Assists += rng.Next(0, 10);
                stat.Rebounds += rng.Next(0, 15);
            }

            stat.LastUpdated = DateTime.UtcNow;
            updated.Add(stat);
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Simulated and persisted stats for {Count} players", updated.Count);
        return updated;
    }
}