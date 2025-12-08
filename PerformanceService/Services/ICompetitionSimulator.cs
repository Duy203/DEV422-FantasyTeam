using PerformanceService.Models;

namespace PerformanceService.Services;

public interface ICompetitionSimulator
{
    Task<List<PerformanceStat>> SimulateAndPersistAsync(List<int>? playerIds, int games);
}