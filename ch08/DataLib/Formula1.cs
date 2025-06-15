namespace DataLib;
public class Formula1
{
    private static List<Team>? s_teams;
    public static IList<Team> GetConstructorChampions() => s_teams ??=
    [
        new("Alfa Romeo", "Italy"),
        new("Maserati", "Italy"),
        new("Vanwall", "United Kingdom", 1958),
        new("Cooper", "United Kingdom", 1959, 1960),
        new("BRM", "United Kingdom", 1962),
        new("Matra", "France", 1969),
        new("Brabham", "United Kingdom", 1966, 1967),
        new("Tyrrell", "United Kingdom", 1971),
        new("Lotus", "United Kingdom", 1963, 1965, 1968, 1970, 1972, 1973, 1978),
        new("Benetton", "Italy", 1995),
        new("Williams", "United Kingdom", 1980, 1981, 1986, 1987, 1992, 1993, 1994, 1996, 1997),
        new("McLaren", "United Kingdom", 1974, 1984, 1985, 1988, 1989, 1990, 1991, 1998, 2024),
        new("Renault", "France", 2005, 2006),
        new("Ferrari", "Italy", 1961, 1964, 1975, 1976, 1977, 1979, 1982, 1983, 1999, 2000, 2001, 2002, 2003, 2004, 2007, 2008),
        new("Brawn", "United Kingdom", 2009),
        new("Red Bull Racing", "Austria", 2010, 2011, 2012, 2013, 2022, 2023),
        new("Mercedes", "Germany", 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021)
    ];

    private static readonly Dictionary<string, Team> s_teamLookup =
      GetConstructorChampions().ToDictionary(
          t => t.Name, StringComparer.OrdinalIgnoreCase);

    private static Team GetTeam(string name) =>
        s_teamLookup.TryGetValue(name, out var team)
            ? team
            : throw new InvalidOperationException($"Team '{name}' not found.");

    private static List<Racer> s_champions;
    public static IEnumerable<Racer> GetFormula1Champions() => s_champions ??=
    [

        // 1950s champions...
        new("Giuseppe", "Farina", "Italy", new DateOnly(1906, 10, 30), new DateOnly(1966, 6, 30))
        {
            Wins = 5,
            PolePositions = 5,
            Championships = [1950],
            Teams = [GetTeam("Alfa Romeo")]
        },
        new("Juan Manuel", "Fangio", "Argentina", new DateOnly(1911, 6, 24), new DateOnly(1995, 7, 17))
        {
            Wins = 24,
            PolePositions = 29,
            Championships = [1951, 1954, 1955, 1956, 1957],
            Teams = [GetTeam("Alfa Romeo"), GetTeam("Maserati"), GetTeam("Mercedes"), GetTeam("Ferrari")]
        },
        new("Alberto", "Ascari", "Italy", new DateOnly(1918, 7, 13), new DateOnly(1955, 5, 26))
        {
            Wins = 13,
            PolePositions = 14,
            Championships = [1952, 1953],
            Teams = [GetTeam("ferrari")]
        },
        new("Mike", "Hawthorn", "United Kingdom", new DateOnly(1929, 4, 10), new DateOnly(1959, 1, 22))
        {
            Wins = 3,
            PolePositions = 4,
            Championships = [1958],
            Teams = [GetTeam("ferrari")]
        },
        new("Jack", "Brabham", "Australia", new DateOnly(1926, 4, 2), new DateOnly(2014, 5, 19))
        {
            Wins = 14,
            PolePositions = 13,
            Championships = [1959, 1960],
            Teams = [GetTeam("cooper")]
        },
        // 1960s champions...
        new("Phil", "Hill", "United States", new DateOnly(1927, 4, 20), new DateOnly(2008, 8, 28))
        {
            Wins = 3,
            PolePositions = 6,
            Championships = [1961],
            Teams = [GetTeam("ferrari")]
        },
        new("Graham", "Hill", "United Kingdom", new DateOnly(1929, 2, 15), new DateOnly(1975, 11, 29))
        {
            Wins = 14,
            PolePositions = 13,
            Championships = [1962, 1968],
            Teams = [GetTeam("BRM"), GetTeam("lotus")]
        },
        new("Jim", "Clark", "United Kingdom", new DateOnly(1936, 3, 4), new DateOnly(1968, 4, 7))
        {
            Wins = 25,
            PolePositions = 33,
            Championships = [1963, 1965],
            Teams = [GetTeam("lotus")]
        },
        new("John", "Surtees", "United Kingdom", new DateOnly(1934, 2, 11), new DateOnly(2017, 3, 10))
        {
            Wins = 6,
            PolePositions = 8,
            Championships = [1964],
            Teams = [GetTeam("ferrari")]
        },
        new("Jack", "Brabham", "Australia", new DateOnly(1926, 4, 2), new DateOnly(2014, 5, 19))
        {
            Wins = 14,
            PolePositions = 13,
            Championships = [1959, 1960, 1966],
            Teams = [GetTeam("Brabham")]
        },
        new("Denny", "Hulme", "New Zealand", new DateOnly(1936, 6, 18), new DateOnly(1992, 10, 4))
        {
            Wins = 8,
            PolePositions = 1,
            Championships = [1967],
            Teams = [GetTeam("Brabham")]
        },
        new("Jackie", "Stewart", "United Kingdom", new DateOnly(1939, 6, 11))
        {
            Wins = 27,
            PolePositions = 17,
            Championships = [1969, 1971, 1973],
            Teams = [GetTeam("Tyrrell"), GetTeam("matra")]
        },
        // 1970s champions
        new("Jochen", "Rindt", "Austria", new DateOnly(1942, 4, 18), new DateOnly(1970, 9, 5))
        {
            Wins = 6,
            PolePositions = 10,
            Championships = [1970],
            Teams = [GetTeam("lotus")]
        },
        new("Emerson", "Fittipaldi", "Brazil", new DateOnly(1946, 12, 12))
        {
            Wins = 14,
            PolePositions = 6,
            Championships = [1972, 1974],
            Teams = [GetTeam("lotus")]
        },
        new("Niki", "Lauda", "Austria", new DateOnly(1949, 2, 22), new DateOnly(2019, 5, 20))
        {
            Wins = 25,
            PolePositions = 24,
            Championships = [1975, 1977, 1984],
            Teams = [GetTeam("Ferrari"), GetTeam("McLaren")]
        },
        new("James", "Hunt", "United Kingdom", new DateOnly(1947, 8, 29), new DateOnly(1993, 6, 15))
        {
            Wins = 10,
            PolePositions = 14,
            Championships = [1976],
            Teams = [GetTeam("McLaren")]
        },
        new("Mario", "Andretti", "United States", new DateOnly(1940, 2, 28))
        {
            Wins = 12,
            PolePositions = 18,
            Championships = [1978],
            Teams = [GetTeam("Lotus")]
        },
        new("Jody", "Scheckter", "South Africa", new DateOnly(1950, 1, 29))
        {
            Wins = 10,
            PolePositions = 3,
            Championships = [1979],
            Teams = [GetTeam("Ferrari")]
        },
        // 1980s champions
        new("Alan", "Jones", "Australia", new DateOnly(1946, 11, 2))
        {
            Wins = 12,
            PolePositions = 6,
            Championships = [1980],
            Teams = [GetTeam("Williams")]
        },
        new("Nelson", "Piquet", "Brazil",  new DateOnly(1952, 8, 17))
        {
            Wins = 23,
            PolePositions = 24,
            Championships = [1981, 1983, 1987],
            Teams = [GetTeam("Brabham")]
        },
        new("Keke", "Rosberg", "Finland", new DateOnly(1948, 12, 6))
        {
            Wins = 5,
            PolePositions = 5,
            Championships = [1982],
            Teams = [GetTeam("Williams")]
        },
        new("Alain", "Prost", "France", new DateOnly(1955, 2, 24))
        {
            Wins = 51,
            PolePositions = 33,
            Championships = [1985, 1986, 1989, 1993],
            Teams = [GetTeam("McLaren")]
        },
        new("Ayrton", "Senna", "Brazil", new DateOnly(1960, 3, 21), new DateOnly(1994, 5, 1))
        {
            Wins = 41,
            PolePositions = 65,
            Championships = [1988, 1990, 1991],
            Teams = [GetTeam("McLaren")]
        },
        // 1990s champions
        new("Nigel", "Mansell", "United Kingdom", new DateOnly(1953, 8, 8))
        {
            Wins = 31,
            PolePositions = 32,
            Championships = [1992],
            Teams = [GetTeam("Williams")]
        },
        new("Michael", "Schumacher", "Germany", new DateOnly(1969, 1, 3))
        {
            Wins = 91,
            PolePositions = 68,
            Championships = [1994, 1995, 2000, 2001, 2002, 2003, 2004],
            Teams = [GetTeam("Benetton"), GetTeam("ferrari")]
        },
        new("Damon", "Hill", "United Kingdom", new DateOnly(1960, 9, 17))
        {
            Wins = 22,
            PolePositions = 20,
            Championships = [1996],
            Teams = [GetTeam("Williams")]
        },
        new("Jacques", "Villeneuve", "Canada", new DateOnly(1971, 4, 9))
        {
            Wins = 11,
            PolePositions = 13,
            Championships = [1997],
            Teams = [GetTeam("Williams")]
        },
        new("Mika", "Häkkinen", "Finland", new DateOnly(1968, 9, 28))
        {
            Wins = 20,
            PolePositions = 26,
            Championships = [1998, 1999],
            Teams = [GetTeam("McLaren")]
        },
        new("Fernando", "Alonso", "Spain", new DateOnly(1981, 7, 29))
        {
            Wins = 32,
            PolePositions = 22,
            Championships = [2005, 2006],
            Teams = [GetTeam("Renault")]
        },
        new("Kimi", "Räikkönen", "Finland", new DateOnly(1979, 10, 17))
        {
            Wins = 21,
            PolePositions = 18,
            Championships = [2007],
            Teams = [GetTeam("Ferrari")]
        },
        new("Lewis", "Hamilton", "United Kingdom", new DateOnly(1985, 1, 7))
        {
            Wins = 103,
            PolePositions = 104,
            Championships = [2008, 2014, 2015, 2017, 2018, 2019, 2020],
            Teams = [GetTeam("McLaren"), GetTeam("mercedes")]
        },
        new("Jenson", "Button", "United Kingdom", new DateOnly(1980, 1, 19))
        {
            Wins = 15,
            PolePositions = 8,
            Championships = [2009],
            Teams = [GetTeam("Brawn")]
        },
        new("Sebastian", "Vettel", "Germany", new DateOnly(1987, 7, 3))
        {
            Wins = 53,
            PolePositions = 57,
            Championships = [2010, 2011, 2012, 2013],
            Teams = [GetTeam("Red Bull Racing")]
        },
        new("Nico", "Rosberg", "Germany", new DateOnly(1985, 6, 27))
        {
            Wins = 23,
            PolePositions = 30,
            Championships = [2016],
            Teams = [GetTeam("Mercedes")]
        },
        new("Max", "Verstappen", "Netherlands", new DateOnly(1997, 9, 30))
        {
            Wins = 54,
            PolePositions = 32,
            Championships = [2021, 2022, 2023, 2024],
            Teams = [GetTeam("Red Bull Racing")]
        }
    ];
}
