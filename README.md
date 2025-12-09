# DEV422-FantasyTeam

# Migration and local dev
*	Add EF Core packages: dotnet add package Microsoft.EntityFrameworkCore.SqlServer and dotnet add package Microsoft.EntityFrameworkCore.Design.
*	Create migration and update DB (locally): dotnet ef migrations add InitialCreate then dotnet ef database update.
*	In Visual Studio you can use Package Manager Console with Add-Migration InitialCreate and Update-Database.
# Deployment & Integration notes
*	Each service (Team, Player, Performance, Leaderboard) must be deployed separately to Azure App Service with its own URL.
*	Store connection strings and external service base URLs in App Service Application settings (environment variables). Do not hardcode secrets.
*	Notify Leaderboard via the configured URL. For production, add retry/backoff and consider Azure Service Bus / Event Grid for reliable async delivery.
*	Secure APIs using API keys / JWT / Azure AD. For simple environment use an API key header checked by middleware.
*	Use DB schema or table name prefixing if you host all tables in same Azure SQL DB. The sample uses table PerformanceStats owned by PerformanceService.
# Testing
*	Test simulate endpoint: POST /api/performance/simulate with body: { "PlayerIds": [1,2,3], "Games": 1 }.
*	Verify performance rows created and Leaderboard receives the POST payload.
*	Integration test flow: create team (Team Service) -> draft player (Player Service) -> simulate performance (Performance Service) -> ensure Leaderboard updated.