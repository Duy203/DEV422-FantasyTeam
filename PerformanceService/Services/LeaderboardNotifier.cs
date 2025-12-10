using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace PerformanceService.Services;

public class LeaderboardNotifier : ILeaderboardNotifier
{
    private readonly HttpClient _http;
    private readonly ILogger<LeaderboardNotifier> _logger;

    public LeaderboardNotifier(HttpClient http, ILogger<LeaderboardNotifier> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task NotifyAsync(IEnumerable<object> updatedStats)
    {
        try
        {
            // POST to /api/leaderboard/updates (adjust path as your Leaderboard uses)
            var resp = await _http.PostAsJsonAsync("/api/leaderboard/updates", updatedStats);
            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogWarning("Leaderboard notification failed with {StatusCode}", resp.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notifying leaderboard");
            // swallow or rethrow based on durability requirements - consider retry policies
        }
    }
}