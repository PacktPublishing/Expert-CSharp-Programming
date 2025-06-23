using Microsoft.EntityFrameworkCore;

namespace Ch08.DataLib;

public static class Formula1DataSeeder
{
    public static async Task SeedDataAsync(Formula1DataContext context, CancellationToken cancellationToken = default)
    {


        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (await context.Teams.AnyAsync(cancellationToken) || await context.Racers.AnyAsync(cancellationToken))
        {
            return; // Data already seeded
        }

        // Execute all seeding operations within a single transaction
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var teams = GetTeams().ToList();
            context.Teams.AddRange(teams);
            await context.SaveChangesAsync(cancellationToken);

            var racers = GetRacers().ToList();
            context.Racers.AddRange(racers);
            await context.SaveChangesAsync(cancellationToken);

            var racerTeamMaps = GetRacersTeamsMap(teams, racers);
            context.RacersTeamsMap.AddRange(racerTeamMaps);
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public static IEnumerable<Team> GetTeams()
    {
        List<Team> teams =
        [
            new() { Name = "Alfa Romeo", Country = "Italy", FoundedYear = 1950 },
            new() { Name = "Maserati", Country = "Italy", FoundedYear = 1950 },
            new() { Name = "Vanwall", Country = "United Kingdom", FoundedYear = 1958 },
            new() { Name = "Cooper", Country = "United Kingdom", FoundedYear = 1959 },
            new() { Name = "BRM", Country = "United Kingdom", FoundedYear = 1962 },
            new() { Name = "Matra", Country = "France", FoundedYear = 1969 },
            new() { Name = "Brabham", Country = "United Kingdom", FoundedYear = 1966 },
            new() { Name = "Tyrrell", Country = "United Kingdom", FoundedYear = 1971 },
            new() { Name = "Lotus", Country = "United Kingdom", FoundedYear = 1963 },
            new() { Name = "Benetton", Country = "Italy", FoundedYear = 1995 },
            new() { Name = "Williams", Country = "United Kingdom", FoundedYear = 1980 },
            new() { Name = "McLaren", Country = "United Kingdom", FoundedYear = 1974 },
            new() { Name = "Renault", Country = "France", FoundedYear = 2005 },
            new() { Name = "Ferrari", Country = "Italy", FoundedYear = 1961 },
            new() { Name = "Brawn", Country = "United Kingdom", FoundedYear = 2009 },
            new() { Name = "Red Bull Racing", Country = "Austria", FoundedYear = 2010 },
            new() { Name = "Mercedes", Country = "Germany", FoundedYear = 2014 }
        ];
        return teams;
    }

    public static IEnumerable<Racer> GetRacers()
    {
        List<Racer> racers =
        [
            new() { FirstName = "Nino", LastName = "Farina", Country = "Italy", BirthDay = new DateOnly(1906, 10, 30),
                DayOfDeath = new DateOnly(1966, 6, 30), Wins = 5 },
            new() { FirstName = "Juan Manuel", LastName = "Fangio", Country = "Argentina", BirthDay = new DateOnly(1911, 6, 24),
                DayOfDeath = new DateOnly(1995, 7, 17), Wins = 24 },
            new() { FirstName = "Michael", LastName = "Schumacher", Country = "Germany", BirthDay = new DateOnly(1969, 1, 3),
                Wins = 91 },
            new() { FirstName = "Lewis", LastName = "Hamilton", Country = "United Kingdom", BirthDay = new DateOnly(1985, 1, 7),
                Wins = 103 },
            new() { FirstName = "Sebastian", LastName = "Vettel", Country = "Germany", BirthDay = new DateOnly(1987, 7, 3),
                Wins = 53 },
            new() { FirstName = "Max", LastName = "Verstappen", Country = "Netherlands", BirthDay = new DateOnly(1997, 9, 30),
                Wins = 54 },
            new() { FirstName = "Ayrton", LastName = "Senna", Country = "Brazil", BirthDay = new DateOnly(1960, 3, 21),
                DayOfDeath = new DateOnly(1994, 5, 1), Wins = 41 },
            new() { FirstName = "Alain", LastName = "Prost", Country = "France", BirthDay = new DateOnly(1955, 2, 24),
                Wins = 51 },
            new() { FirstName = "Fernando", LastName = "Alonso", Country = "Spain", BirthDay = new DateOnly(1981, 7, 29),
                Wins = 32 },
            new() { FirstName = "Niki", LastName = "Lauda", Country = "Austria", BirthDay = new DateOnly(1949, 2, 22),
                DayOfDeath = new DateOnly(2019, 5, 20), Wins = 25 }
        ];
        return racers;
    }

    private static IEnumerable<RacerTeamMap> GetRacersTeamsMap(List<Team> teams, List<Racer> racers)
    {
        var ferrari = teams.First(t => t.Name == "Ferrari");
        var mercedes = teams.First(t => t.Name == "Mercedes");
        var redbull = teams.First(t => t.Name == "Red Bull Racing");
        var mclaren = teams.First(t => t.Name == "McLaren");
        var williams = teams.First(t => t.Name == "Williams");
        var benetton = teams.First(t => t.Name == "Benetton");

        var schumacher = racers.First(r => r.LastName == "Schumacher");
        var hamilton = racers.First(r => r.LastName == "Hamilton");
        var verstappen = racers.First(r => r.LastName == "Verstappen");
        var vettel = racers.First(r => r.LastName == "Vettel");
        var senna = racers.First(r => r.LastName == "Senna");
        var prost = racers.First(r => r.LastName == "Prost");

        List<RacerTeamMap> maps =
        [
            // Schumacher's career
            new(benetton.Id, schumacher.Id, 1994),
            new(benetton.Id, schumacher.Id, 1995),
            new(ferrari.Id, schumacher.Id, 1996),
            new(ferrari.Id, schumacher.Id, 2000),
            new(ferrari.Id, schumacher.Id, 2004),

            // Hamilton's career
            new(mclaren.Id, hamilton.Id, 2007),
            new(mclaren.Id, hamilton.Id, 2008),
            new(mercedes.Id, hamilton.Id, 2013),
            new(mercedes.Id, hamilton.Id, 2014),
            new(mercedes.Id, hamilton.Id, 2020),

            // Verstappen's career
            new(redbull.Id, verstappen.Id, 2016),
            new(redbull.Id, verstappen.Id, 2021),
            new(redbull.Id, verstappen.Id, 2022),
            new(redbull.Id, verstappen.Id, 2023),

            // Vettel's career
            new(redbull.Id, vettel.Id, 2010),
            new(redbull.Id, vettel.Id, 2013),
            new(ferrari.Id, vettel.Id, 2015),
            new(ferrari.Id, vettel.Id, 2019),

            // Senna's career
            new(mclaren.Id, senna.Id, 1988),
            new(mclaren.Id, senna.Id, 1991),
            new(williams.Id, senna.Id, 1994),

            // Prost's career
            new(mclaren.Id, prost.Id, 1985),
            new(mclaren.Id, prost.Id, 1986),
            new(ferrari.Id, prost.Id, 1990),
            new(williams.Id, prost.Id, 1993)
        ];

        return maps;
    }
}