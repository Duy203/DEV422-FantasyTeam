using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformanceService.Models;

[Table("PerformanceStats")]
public class PerformanceStat
{
    [Key]
    public int Id { get; set; }

    // PlayerId references an external player (no FK at DB level)
    public int PlayerId { get; set; }

    public int GamesPlayed { get; set; }

    public int Points { get; set; }

    public int Assists { get; set; }

    public int Rebounds { get; set; }

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}