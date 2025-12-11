namespace DEV422_FantasyTeam.Services
{
    public interface IPerformanceService
    {
        Task<object?> SimulateAsync(List<int> playerIds, int games);
    }
}
