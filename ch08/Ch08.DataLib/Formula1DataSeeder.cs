using Microsoft.EntityFrameworkCore;

namespace Ch08.DataLib;

public static class Formula1DataSeeder
{
    public static async Task SeedDataAsync(Formula1DataContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Teams.AnyAsync() || await context.Racers.AnyAsync())
        {
            return; // Data already seeded
        }

        // Add teams first
        var teams = new List<Team>
        {
            new Team { Name = "Mercedes", Country = "Germany", FoundedYear = 1954 },
            new Team { Name = "Ferrari", Country = "Italy", FoundedYear = 1950 },
            new Team { Name = "Red Bull Racing", Country = "Austria", FoundedYear = 2005 },
            new Team { Name = "McLaren", Country = "United Kingdom", FoundedYear = 1963 },
            new Team { Name = "Lotus", Country = "United Kingdom", FoundedYear = 1958 },
            new Team { Name = "Williams", Country = "United Kingdom", FoundedYear = 1977 },
            new Team { Name = "Force India", Country = "India", FoundedYear = 2008 },
            new Team { Name = "Sauber", Country = "Switzerland", FoundedYear = 1993 },
            new Team { Name = "Toro Rosso", Country = "Italy", FoundedYear = 2006 },
            new Team { Name = "Caterham", Country = "Malaysia", FoundedYear = 2010 },
            new Team { Name = "HRT", Country = "Spain", FoundedYear = 2010 },
            new Team { Name = "Marussia", Country = "Russia", FoundedYear = 2010 }
        };

        context.Teams.AddRange(teams);
        await context.SaveChangesAsync();

        // Add racers with team references
        var racers = new List<Racer>
        {
            new Racer { FirstName = "Lewis", LastName = "Hamilton", Country = "United Kingdom", BirthDay = new DateOnly(1985, 1, 7), NumberWins = 103, TeamId = teams.First(t => t.Name == "Mercedes").Id },
            new Racer { FirstName = "Michael", LastName = "Schumacher", Country = "Germany", BirthDay = new DateOnly(1969, 1, 3), NumberWins = 91, TeamId = teams.First(t => t.Name == "Mercedes").Id },
            new Racer { FirstName = "Sebastian", LastName = "Vettel", Country = "Germany", BirthDay = new DateOnly(1987, 7, 3), NumberWins = 53, TeamId = teams.First(t => t.Name == "Red Bull Racing").Id },
            new Racer { FirstName = "Ayrton", LastName = "Senna", Country = "Brazil", BirthDay = new DateOnly(1960, 3, 21), NumberWins = 41, TeamId = teams.First(t => t.Name == "McLaren").Id },
            new Racer { FirstName = "Fernando", LastName = "Alonso", Country = "Spain", BirthDay = new DateOnly(1981, 7, 29), NumberWins = 32, TeamId = teams.First(t => t.Name == "Ferrari").Id },
            new Racer { FirstName = "Max", LastName = "Verstappen", Country = "Netherlands", BirthDay = new DateOnly(1997, 9, 30), NumberWins = 49, TeamId = teams.First(t => t.Name == "Red Bull Racing").Id },
            new Racer { FirstName = "Niki", LastName = "Lauda", Country = "Austria", BirthDay = new DateOnly(1949, 2, 22), NumberWins = 25, TeamId = teams.First(t => t.Name == "Ferrari").Id },
            new Racer { FirstName = "Alain", LastName = "Prost", Country = "France", BirthDay = new DateOnly(1955, 2, 24), NumberWins = 51, TeamId = teams.First(t => t.Name == "McLaren").Id },
            new Racer { FirstName = "Kimi", LastName = "Räikkönen", Country = "Finland", BirthDay = new DateOnly(1979, 10, 17), NumberWins = 21, TeamId = teams.First(t => t.Name == "Ferrari").Id },
            new Racer { FirstName = "Jenson", LastName = "Button", Country = "United Kingdom", BirthDay = new DateOnly(1980, 1, 19), NumberWins = 15, TeamId = teams.First(t => t.Name == "McLaren").Id },
            new Racer { FirstName = "Nigel", LastName = "Mansell", Country = "United Kingdom", BirthDay = new DateOnly(1953, 8, 8), NumberWins = 31, TeamId = teams.First(t => t.Name == "Williams").Id },
            new Racer { FirstName = "Nelson", LastName = "Piquet", Country = "Brazil", BirthDay = new DateOnly(1952, 8, 17), NumberWins = 23, TeamId = teams.First(t => t.Name == "Williams").Id },
            new Racer { FirstName = "Jackie", LastName = "Stewart", Country = "United Kingdom", BirthDay = new DateOnly(1939, 6, 11), NumberWins = 27 },
            new Racer { FirstName = "James", LastName = "Hunt", Country = "United Kingdom", BirthDay = new DateOnly(1947, 8, 29), NumberWins = 10, TeamId = teams.First(t => t.Name == "McLaren").Id },
            new Racer { FirstName = "Juan Manuel", LastName = "Fangio", Country = "Argentina", BirthDay = new DateOnly(1911, 6, 24), NumberWins = 24 },
            new Racer { FirstName = "Graham", LastName = "Hill", Country = "United Kingdom", BirthDay = new DateOnly(1929, 2, 15), NumberWins = 14, TeamId = teams.First(t => t.Name == "Lotus").Id },
            new Racer { FirstName = "Damon", LastName = "Hill", Country = "United Kingdom", BirthDay = new DateOnly(1960, 9, 17), NumberWins = 22, TeamId = teams.First(t => t.Name == "Williams").Id },
            new Racer { FirstName = "Jacques", LastName = "Villeneuve", Country = "Canada", BirthDay = new DateOnly(1971, 4, 9), NumberWins = 11, TeamId = teams.First(t => t.Name == "Williams").Id },
            new Racer { FirstName = "David", LastName = "Coulthard", Country = "United Kingdom", BirthDay = new DateOnly(1971, 3, 27), NumberWins = 13, TeamId = teams.First(t => t.Name == "McLaren").Id },
            new Racer { FirstName = "Rubens", LastName = "Barrichello", Country = "Brazil", BirthDay = new DateOnly(1972, 5, 23), NumberWins = 11, TeamId = teams.First(t => t.Name == "Ferrari").Id },
            new Racer { FirstName = "Valtteri", LastName = "Bottas", Country = "Finland", BirthDay = new DateOnly(1989, 8, 28), NumberWins = 10, TeamId = teams.First(t => t.Name == "Mercedes").Id },
            new Racer { FirstName = "Sergio", LastName = "Perez", Country = "Mexico", BirthDay = new DateOnly(1990, 1, 26), NumberWins = 6, TeamId = teams.First(t => t.Name == "Red Bull Racing").Id },
            new Racer { FirstName = "George", LastName = "Russell", Country = "United Kingdom", BirthDay = new DateOnly(1998, 2, 15), NumberWins = 1, TeamId = teams.First(t => t.Name == "Mercedes").Id },
            new Racer { FirstName = "Daniel", LastName = "Ricciardo", Country = "Australia", BirthDay = new DateOnly(1989, 7, 1), NumberWins = 8, TeamId = teams.First(t => t.Name == "McLaren").Id },
            new Racer { FirstName = "Charles", LastName = "Leclerc", Country = "Monaco", BirthDay = new DateOnly(1997, 10, 16), NumberWins = 5, TeamId = teams.First(t => t.Name == "Ferrari").Id },
            new Racer { FirstName = "Carlos", LastName = "Sainz", Country = "Spain", BirthDay = new DateOnly(1994, 9, 1), NumberWins = 1, TeamId = teams.First(t => t.Name == "Ferrari").Id }
        };

        context.Racers.AddRange(racers);
        await context.SaveChangesAsync();
    }
}