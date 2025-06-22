using Ch08.DataLib;
using Microsoft.EntityFrameworkCore;

// Test the repository functionality
var optionsBuilder = new DbContextOptionsBuilder<Formula1DataContext>();
optionsBuilder.UseSqlite("Data Source=test_formula1.db");

var sqlLogger = new SqlQueryLogger();
var sqlInterceptor = new SqlLoggingInterceptor(sqlLogger);
optionsBuilder.AddInterceptors(sqlInterceptor);

using var context = new Formula1DataContext(optionsBuilder.Options);
await Formula1DataSeeder.SeedDataAsync(context);

var repository = new Formula1Repository(context, sqlLogger);

// Test different queries
Console.WriteLine("=== Testing Repository Queries ===\n");

// Test 1: Get all racers
Console.WriteLine("1. Getting all racers...");
var allRacers = await repository.GetRacersAsync();
Console.WriteLine($"Found {allRacers.Count()} racers");
Console.WriteLine($"Last SQL: {repository.GetLastExecutedSql()}\n");

// Test 2: Get racers by country
Console.WriteLine("2. Getting racers from United Kingdom...");
var ukRacers = await repository.GetRacersByCountryAsync("United Kingdom");
Console.WriteLine($"Found {ukRacers.Count()} UK racers");
Console.WriteLine($"Last SQL: {repository.GetLastExecutedSql()}\n");

// Test 3: Get racers with minimum wins
Console.WriteLine("3. Getting racers with at least 20 wins...");
var topRacers = await repository.GetRacersWithMostWinsAsync(20);
Console.WriteLine($"Found {topRacers.Count()} top racers");
Console.WriteLine($"Last SQL: {repository.GetLastExecutedSql()}\n");

// Test 4: Group by country
Console.WriteLine("4. Getting racers grouped by country...");
var groupedRacers = await repository.GetRacersGroupedByCountryAsync();
Console.WriteLine($"Found {groupedRacers.Count()} countries");
Console.WriteLine($"Last SQL: {repository.GetLastExecutedSql()}\n");

Console.WriteLine("All tests completed successfully!");
