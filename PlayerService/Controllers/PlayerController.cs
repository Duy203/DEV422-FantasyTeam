using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.Data;
using PlayerService.Models;

namespace PlayerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerContext _context;

        public PlayerController(PlayerContext context)
        {
            _context = context;
        }

        // GET all players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET player by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound();

            return player;
        }

        // DRAFT player to a team
        [HttpPut("{id}/draft/{teamId}")]
        public async Task<ActionResult<Player>> DraftPlayer(int id, int teamId)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound($"Player ID {id} not found");

            if (player.IsDrafted)
                return Conflict($"Player {id} is already drafted by team {player.TeamId}");

            player.IsDrafted = true;
            player.TeamId = teamId;

            await _context.SaveChangesAsync();

            return Ok(player);
        }

        // RELEASE player from team
        [HttpPut("{id}/release")]
        public async Task<ActionResult<Player>> ReleasePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return NotFound($"Player ID {id} not found");

            if (!player.IsDrafted)
                return BadRequest($"Player {id} is not drafted");

            player.IsDrafted = false;
            player.TeamId = null;

            await _context.SaveChangesAsync();

            return Ok(player);
        }
    }
}
