namespace DEV422_FantasyTeam.Services
{
    public interface IPlayerService
    {
        Task<object?> GetPlayerById(int playerId);
        Task<object?> DraftPlayer(int playerId, int teamId);
        Task<object?> ReleasePlayer(int playerId);
    }
}
