using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DEV422_FantasyTeam.Data;
using DEV422_FantasyTeam.Services;
using DEV422_FantasyTeam.DTOs;
using DEV422_FantasyTeam.Models;
using System.Linq.Expressions;



namespace DEV422_FantasyTeam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPlayerService _playerservice;

        public TeamsController(AppDbContext context, IPlayerService playerservice)
        {
            _context = context;
            _playerservice = playerservice;
        }

        //Team Create
        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamDto dto)
        {
            var team = new Team { TeamName = dto.TeamName
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return Ok(team);
        }

        //Get Team and its players
        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeam(int teamId)
        {
            var team = await _context.Teams.FindAsync(teamId);
            if (team == null) return NotFound("Team not Found");

            var links = await _context.TeamPlayers
                .Where(tp => tp.TeamId == teamId)
                .ToListAsync();

            var players = new List<object>();

            foreach (var link in links)
            {
                var p = await _playerservice.GetPlayerById(link.PlayerId);
                if (p != null)
                {
                    players.Add(p);
                }
            }

            return Ok(new TeamResponseDto
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Players = players
            });
        }

        //Draft Player into teams
        [HttpPost("{teamId}/draft/{playerId}")]
        public async Task<IActionResult> DraftPlayer(int teamId,int playerId)
        {
            var TeamExsit = await _context.Teams.AnyAsync(t => t.TeamId == teamId);
            if (!TeamExsit) return NotFound("Team Not Found");

            var result = await _playerservice.DraftPlayer(playerId, teamId);
            if (result == null) return BadRequest("Draft failed. Player might be drafted");

            _context.TeamPlayers.Add(new TeamPlayer
            {
                TeamId = teamId,
                PlayerId = playerId
            });

            await _context.SaveChangesAsync();
            return Ok(result);
        }

        // RELEASE PLAYER FROM TEAM
        [HttpPost("{teamId}/release/{playerId}")]
        public async Task<IActionResult> ReleasePlayer(int teamId, int playerId)
        {
            var link = await _context.TeamPlayers
                .FirstOrDefaultAsync(tp => tp.TeamId == teamId && tp.PlayerId == playerId);

            if (link == null)
                return NotFound("Player not found in this team.");

            var result = await _playerservice.ReleasePlayer(playerId);

            _context.TeamPlayers.Remove(link);
            await _context.SaveChangesAsync();

            return Ok(result);
        }
    }
}
