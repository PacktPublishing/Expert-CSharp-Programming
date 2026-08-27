# Chapter 8 - EF Core Sample with SQL Query Generation

This sample demonstrates Entity Framework Core with database providers, showing generated SQL queries in a Blazor Server application.

## Features

- **Blazor Server Application**: Interactive UI for executing database queries
- **EF Core with SQLite/PostgreSQL**: Database context with Formula 1 data
- **Repository Pattern**: Clean data access layer
- **SQL Query Logging**: Real-time display of generated SQL queries
- **Sample Data**: Formula 1 champions and teams data
- **Test Console**: Console application to verify queries

## Project Structure

- `Ch08.DataLib/`: Contains EF Core models, context, and repository
- `Ch08.BlazorApp/`: Blazor Server application with query UI
- `Ch08.TestConsole/`: Console application for testing queries
- `Ch08.AppHost/`: Aspire orchestration (optional, requires Aspire workload)

## Setup Instructions

### Option 1: Using SQLite (Default - Ready to Run)

1. Run the Blazor application:
   ```bash
   cd ch08
   dotnet run --project Ch08.BlazorApp
   ```

2. Or test the queries directly:
   ```bash
   cd ch08
   dotnet run --project Ch08.TestConsole
   ```

### Option 2: Using PostgreSQL with Docker

1. Switch to PostgreSQL by updating the project files:
   - Change `Ch08.DataLib.csproj` to use `Npgsql.EntityFrameworkCore.PostgreSQL`
   - Change `Ch08.BlazorApp.csproj` to use `Npgsql.EntityFrameworkCore.PostgreSQL`
   - Update `Program.cs` to use `UseNpgsql`
   - Update connection string in `appsettings.json`

2. Start PostgreSQL using Docker Compose:
   ```bash
   docker-compose up -d
   ```

3. Run the application:
   ```bash
   dotnet run --project Ch08.BlazorApp
   ```

## Usage

### Blazor Application
1. Navigate to `http://localhost:5xxx` (port varies)
2. Click on "Formula 1 Queries" in the navigation
3. Select a query type from the dropdown
4. Enter parameters if required (country name, minimum wins)
5. Click "Execute Query" to run the query
6. View the results and the generated SQL query

### Test Console
Run `dotnet run --project Ch08.TestConsole` to see various queries executed with their SQL output.

## Query Types

- **All Racers**: Get all racers with their teams
- **All Teams**: Get all teams with their racers
- **Racers by Country**: Filter racers by country
- **Racers with Min Wins**: Filter racers by minimum number of wins
- **Racers Grouped by Country**: Group racers by country with statistics
- **Teams with Racer Count**: Teams with racer count and total wins

## Example Generated SQL

```sql
-- Get all racers with teams
SELECT "r"."Id", "r"."BirthDay", "r"."Country", "r"."FirstName", 
       "r"."LastName", "r"."NumberWins", "r"."TeamId", 
       "t"."Id", "t"."Country", "t"."FoundedYear", "t"."Name"
FROM "Racers" AS "r"
LEFT JOIN "Teams" AS "t" ON "r"."TeamId" = "t"."Id"

-- Get racers by country (parameterized)
SELECT "r"."Id", "r"."BirthDay", "r"."Country", "r"."FirstName", 
       "r"."LastName", "r"."NumberWins", "r"."TeamId", 
       "t"."Id", "t"."Country", "t"."FoundedYear", "t"."Name"
FROM "Racers" AS "r"
LEFT JOIN "Teams" AS "t" ON "r"."TeamId" = "t"."Id"
WHERE "r"."Country" = @__country_0

-- Group racers by country
SELECT "r"."Country", COUNT(*) AS "Count", 
       COALESCE(SUM("r"."NumberWins"), 0) AS "TotalWins"
FROM "Racers" AS "r"
GROUP BY "r"."Country"
ORDER BY COUNT(*) DESC
```

## Technologies Used

- .NET 8
- Entity Framework Core
- SQLite (default) / PostgreSQL (optional)
- Blazor Server
- Bootstrap
- SQL Query Interceptors for logging