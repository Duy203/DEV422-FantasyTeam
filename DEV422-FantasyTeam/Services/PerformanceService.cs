using System.Net.Http.Json;

namespace DEV422_FantasyTeam.Services
{
    public class PerformanceService : IPerformanceService
    {
        private readonly HttpClient _http;

        public PerformanceService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config["PerformanceService:BaseUrl"]!);
        }

        public async Task<object?> SimulateAsync(List<int> playerIds, int games)
        {
            var body = new
            {
                PlayerIds = playerIds,
                Games = games
            };

            var response = await _http.PostAsJsonAsync("/api/performance/simulate", body);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<object>();
        }
    }
}
