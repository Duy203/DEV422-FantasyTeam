namespace PerformanceService.DTOs;

public class SimulateRequestDto
{
    // If empty, simulate a batch of random players (requires service to choose).
    public List<int>? PlayerIds { get; set; }

    // Number of simulated games per player
    public int Games { get; set; } = 1;
}