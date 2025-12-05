using Microsoft.EntityFrameworkCore;
using DEV422_FantasyTeam.Models;
namespace DEV422_FantasyTeam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
    }
}
