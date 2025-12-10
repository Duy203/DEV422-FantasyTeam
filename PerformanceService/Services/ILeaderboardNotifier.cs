namespace PerformanceService.Services;

public interface ILeaderboardNotifier
{
    Task NotifyAsync(IEnumerable<object> updatedStats);
}