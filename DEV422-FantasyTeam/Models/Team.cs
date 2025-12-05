namespace DEV422_FantasyTeam.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
