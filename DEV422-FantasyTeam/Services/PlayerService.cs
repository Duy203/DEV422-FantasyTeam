using System.Net.Http.Json;
using DEV422_FantasyTeam.Services;
namespace DEV422_FantasyTeam.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly HttpClient _httpClient;

        public PlayerService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config["PlayerService:BaseUrl"]!);
        }

        public async Task<object?> GetPlayerById(int playerId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<object>($"/{playerId}");
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<object?> DraftPlayer(int playerId, int teamId)
        {
            var response = await _httpClient.PutAsync($"/{playerId}/release",null);
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<object>();
        }

        public async Task<object?> ReleasePlayer(int playerId)
        {
            var response = await _httpClient.PutAsync($"/{playerId}/release", null);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}
