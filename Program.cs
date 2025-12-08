using Microsoft.EntityFrameworkCore;
using PerformanceService.Data;
using PerformanceService.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<PerformanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Leaderboard notifier (uses typed HttpClient)
builder.Services.AddHttpClient<ILeaderboardNotifier, LeaderboardNotifier>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["LeaderboardService:BaseUrl"] ?? "https://leaderboard/");
});

// Add scoped services
builder.Services.AddScoped<ICompetitionSimulator, CompetitionSimulator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();