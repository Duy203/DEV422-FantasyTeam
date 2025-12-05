using Microsoft.EntityFrameworkCore;
using PlayerService.Models;

namespace PlayerService.Data
{
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options) : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasData(
                new Player { PlayerId = 1, Name = "Tom Brady", Position = "QB", IsDrafted = false },
                new Player { PlayerId = 2, Name = "Derrick Henry", Position = "RB", IsDrafted = false },
                new Player { PlayerId = 3, Name = "Davante Adams", Position = "WR", IsDrafted = false },
                new Player { PlayerId = 4, Name = "Travis Kelce", Position = "TE", IsDrafted = false },
                new Player { PlayerId = 5, Name = "Patrick Mahomes", Position = "QB", IsDrafted = false }
            );
        }
    }
}
