using Microsoft.EntityFrameworkCore;
using PerformanceService.Models;

namespace PerformanceService.Data;

public class PerformanceDbContext : DbContext
{
    public PerformanceDbContext(DbContextOptions<PerformanceDbContext> options) : base(options) { }

    public DbSet<PerformanceStat> PerformanceStats { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerformanceStat>()
            .HasIndex(p => p.PlayerId)
            .IsUnique();
    }
}