using DEV422_FantasyTeam.Services;
using System.Net.Http;
using System.Net.Http.Json;
namespace DEV422_FantasyTeam.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly HttpClient _httpClient;

        public PlayerService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config["PlayerService:BaseUrl"]!);

            // Add API KEY header
            var apiKey = config["ApiKey"];
            _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
        }

        public async Task<object?> GetPlayerById(int playerId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<object>($"/api/player/{playerId}");
                //return await _httpClient.GetFromJsonAsync<object>($"/{playerId}");
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<object?> DraftPlayer(int playerId, int teamId)
        {
            var response = await _httpClient.PutAsync($"/api/player/{playerId}/draft/{teamId}", null);
            //var response = await _httpClient.PutAsync($"/{playerId}/draft/{teamId}",null);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<object>();
        }

        public async Task<object?> ReleasePlayer(int playerId)
        {
            var response = await _httpClient.PutAsync($"/api/player/{playerId}/release", null);

            //var response = await _httpClient.PutAsync($"/{playerId}/release", null);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}
