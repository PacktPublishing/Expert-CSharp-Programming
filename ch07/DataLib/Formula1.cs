namespace Data;

public class Formula1
{
    private readonly static List<Racer> s_racers =
    [
        new Racer("Lewis", "Hamilton", "United Kingdom", new DateOnly(1985, 1, 7), 103),
        new Racer("Michael", "Schumacher", "Germany", new DateOnly(1969, 1, 3), 91),
        new Racer("Sebastian", "Vettel", "Germany", new DateOnly(1987, 7, 3), 53),
        new Racer("Ayrton", "Senna", "Brazil", new DateOnly(1960, 3, 21), 41),
        new Racer("Fernando", "Alonso", "Spain", new DateOnly(1981, 7, 29), 32),
        new Racer("Max", "Verstappen", "Netherlands", new DateOnly(1997, 9, 30), 49),
        new Racer("Niki", "Lauda", "Austria", new DateOnly(1949, 2, 22), 25),
        new Racer("Alain", "Prost", "France", new DateOnly(1955, 2, 24), 51),
        new Racer("Kimi", "Räikkönen", "Finland", new DateOnly(1979, 10, 17), 21),
        new Racer("Jenson", "Button", "United Kingdom", new DateOnly(1980, 1, 19), 15),
        new Racer("Nigel", "Mansell", "United Kingdom", new DateOnly(1953, 8, 8), 31),
        new Racer("Nelson", "Piquet", "Brazil", new DateOnly(1952, 8, 17), 23),
        new Racer("Jackie", "Stewart", "United Kingdom", new DateOnly(1939, 6, 11), 27),
        new Racer("James", "Hunt", "United Kingdom", new DateOnly(1947, 8, 29), 10),
        new Racer("Juan Manuel", "Fangio", "Argentina", new DateOnly(1911, 6, 24), 24),
        new Racer("Graham", "Hill", "United Kingdom", new DateOnly(1929, 2, 15), 14),
        new Racer("Damon", "Hill", "United Kingdom", new DateOnly(1960, 9, 17), 22),
        new Racer("Jacques", "Villeneuve", "Canada", new DateOnly(1971, 4, 9), 11),
        new Racer("David", "Coulthard", "United Kingdom", new DateOnly(1971, 3, 27), 13),
        new Racer("Rubens", "Barrichello", "Brazil", new DateOnly(1972, 5, 23), 11),
        new Racer("Valtteri", "Bottas", "Finland", new DateOnly(1989, 8, 28), 10),
        new Racer("Sergio", "Perez", "Mexico", new DateOnly(1990, 1, 26), 6),
        new Racer("George", "Russell", "United Kingdom", new DateOnly(1998, 2, 15), 1),
        new Racer("Daniel", "Ricciardo", "Australia", new DateOnly(1989, 7, 1), 8),
        new Racer("Charles", "Leclerc", "Monaco", new DateOnly(1997, 10, 16), 5),
        new Racer("Carlos", "Sainz", "Spain", new DateOnly(1994, 9, 1), 1),
        new Racer("Jody", "Scheckter", "South Africa", new DateOnly(1950, 1, 29), 10),
        new Racer("Phil", "Hill", "United States", new DateOnly(1927, 4, 20), 3),
        new Racer("Mario", "Andretti", "United States", new DateOnly(1940, 2, 28), 12),
        new Racer("Gerhard", "Berger", "Austria", new DateOnly(1959, 8, 27), 10),
        new Racer("Mika", "Häkkinen", "Finland", new DateOnly(1968, 9, 28), 20),
        new Racer("Emerson", "Fittipaldi", "Brazil", new DateOnly(1946, 12, 12), 14),
        new Racer("Riccardo", "Patrese", "Italy", new DateOnly(1954, 4, 17), 6),
        new Racer("Rene", "Arnoux", "France", new DateOnly(1948, 7, 4), 7),
        new Racer("Keke", "Rosberg", "Finland", new DateOnly(1948, 12, 6), 5),
        new Racer("Nico", "Rosberg", "Germany", new DateOnly(1985, 6, 27), 23),
        new Racer("Heinz-Harald", "Frentzen", "Germany", new DateOnly(1967, 5, 18), 3),
        new Racer("Heikki", "Kovalainen", "Finland", new DateOnly(1981, 10, 19), 1),
        new Racer("Jarno", "Trulli", "Italy", new DateOnly(1974, 7, 13), 1),
        new Racer("Jean", "Alesi", "France", new DateOnly(1964, 6, 11), 1),
    ];

    public static IEnumerable<Racer> GetRacers() => s_racers;

    static Formula1()
    {
        // Generate additional racers programmatically to reach 1000 entries
        //for (int i = 1; i <= 980; i++)
        //{
        //    s_racers.Add(new Racer(
        //        $"FirstName{i}",
        //        $"LastName{i}",
        //        GetRandomCountry(),
        //        GetRandomBirthday(),
        //        GetRandomRaceWins()
        //    ));
        //}
    }

    private static string GetRandomCountry()
    {
        string[] countries = { "United Kingdom", "Germany", "Brazil", "France", "Italy", "Spain", "Netherlands", "Finland", "Austria", "Canada", "Argentina", "Australia", "United States", "Sweden", "Japan" };
        return countries[new Random().Next(countries.Length)];
    }

    private static DateOnly GetRandomBirthday()
    {
        Random random = new();
        int year = random.Next(1930, 2005); // Random year between 1930 and 2005
        int month = random.Next(1, 13);    // Random month
        int day = random.Next(1, 29);      // Random day (to avoid invalid dates)
        return new DateOnly(year, month, day);
    }

    private static int GetRandomRaceWins()
    {
        Random random = new();
        return random.Next(0, 8); // Random number of race wins between 0 and 7
    }
}
