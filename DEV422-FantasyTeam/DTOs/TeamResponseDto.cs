using System.Collections.Generic;

namespace DEV422_FantasyTeam.DTOs
{
    public class TeamResponseDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public List<object> Players { get; set; }
    }
}
