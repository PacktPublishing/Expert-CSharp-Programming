# Chapter 8 - EF Core Sample with SQL Query Generation

This sample demonstrates Entity Framework Core with PostgreSQL, showing generated SQL queries in a Blazor Server application.

## Features

- **Blazor Server Application**: Interactive UI for executing database queries
- **EF Core with PostgreSQL**: Database context with Formula 1 data
- **Repository Pattern**: Clean data access layer
- **SQL Query Logging**: Real-time display of generated SQL queries
- **Sample Data**: Formula 1 champions and teams data

## Project Structure

- `Ch08.DataLib/`: Contains EF Core models, context, and repository
- `Ch08.BlazorApp/`: Blazor Server application with query UI
- `Ch08.AppHost/`: Aspire orchestration (optional, requires Aspire workload)

## Setup Instructions

### Option 1: Using Docker (Recommended)

1. Start PostgreSQL using Docker Compose:
   ```bash
   docker-compose up -d
   ```

2. Run the Blazor application:
   ```bash
   dotnet run --project Ch08.BlazorApp
   ```

### Option 2: Using Local PostgreSQL

1. Install PostgreSQL locally
2. Create a database named `formula1db`
3. Update the connection string in `appsettings.json` if needed
4. Run the application:
   ```bash
   dotnet run --project Ch08.BlazorApp
   ```

## Usage

1. Navigate to `/formula1` in the Blazor application
2. Select a query type from the dropdown
3. Enter parameters if required (country name, minimum wins)
4. Click "Execute Query" to run the query
5. View the results and the generated SQL query

## Query Types

- **All Racers**: Get all racers with their teams
- **All Teams**: Get all teams with their racers
- **Racers by Country**: Filter racers by country
- **Racers with Min Wins**: Filter racers by minimum number of wins
- **Racers Grouped by Country**: Group racers by country with statistics
- **Teams with Racer Count**: Teams with racer count and total wins

## Technologies Used

- .NET 8
- Entity Framework Core
- PostgreSQL
- Blazor Server
- Bootstrap