using Microsoft.EntityFrameworkCore;
using PerformanceService.Data;
using PerformanceService.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DB Context
builder.Services.AddDbContext<PerformanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Register Simulation Engine
builder.Services.AddScoped<ICompetitionSimulator, CompetitionSimulator>();

// 3. Register Leaderboard Notifier + HttpClient (optional if not deployed yet)
builder.Services.AddHttpClient<ILeaderboardNotifier, LeaderboardNotifier>(client =>
{
    var baseUrl = builder.Configuration["LeaderboardService:BaseUrl"];
    if (!string.IsNullOrWhiteSpace(baseUrl))
        client.BaseAddress = new Uri(baseUrl);
});

// 4. Add controllers + swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Enable Swagger in Development
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Performance API v1");
});

app.UseHttpsRedirection();

// 6. Map endpoints
app.MapControllers();

// 7. Apply migrations automatically (creates PerformanceStats table)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PerformanceDbContext>();
    db.Database.Migrate();
}

app.Run();
