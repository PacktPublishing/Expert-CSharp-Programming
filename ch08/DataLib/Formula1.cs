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

    private static readonly Dictionary<string, Team> teamsLookup =
      GetConstructorChampions().ToDictionary(
          t => t.Name, StringComparer.OrdinalIgnoreCase);

    private static Team GetTeam(string name) =>
        teamsLookup.TryGetValue(name, out var team)
            ? team
            : throw new InvalidOperationException($"Team '{name}' not found.");

    private static List<Racer>? s_champions;
    public static IEnumerable<Racer> GetFormula1Champions() => s_champions ??=
    [

        // 1950s champions...
        new("Nino", "Farina", "Italy", new DateOnly(1906, 10, 30), new DateOnly(1966, 6, 30))
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
            Championships = [1959, 1960, 1966],
            Teams = [GetTeam("cooper"), GetTeam("brabham")]
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

    private static List<Racer>? s_moreRacers;
    public static IList<Racer> GetMoreRacers() => s_moreRacers ??=
    [
        new Racer("Luigi", "Fagioli", "Italy", new DateOnly(1898, 6, 9), new DateOnly(1952, 6, 20), Wins: 1),
        new Racer("Jose Froilan", "Gonzalez", "Argentina", new DateOnly(1922, 10, 5), new DateOnly(2013, 6, 15), Wins: 2),
        new Racer("Piero", "Taruffi", "Italy", new DateOnly(1906, 10, 12), new DateOnly(1988, 1, 12), Wins: 1),
        new Racer("Stirling", "Moss", "UK", new DateOnly(1929, 9, 17), new DateOnly(2020, 4, 12), Wins: 16),
        new Racer("Eugenio", "Castellotti", "Italy", new DateOnly(1930, 10, 10), new DateOnly(1957, 3, 14), Wins: 0),
        new Racer("Peter", "Collins", "UK", new DateOnly(1931, 11, 6), new DateOnly(1958, 8, 3), Wins: 3),
        new Racer("Luigi", "Musso", "Italy", new DateOnly(1924, 7, 28), new DateOnly(1958, 7, 6), Wins: 1),
        new Racer("Tony", "Brooks", "UK", new DateOnly(1932, 2, 25), new DateOnly(2022, 5, 3), Wins: 6),
        new Racer("Bruce", "McLaren", "New Zealand", new DateOnly(1937, 8, 30), new DateOnly(1970, 6, 2), Wins: 4),
        new Racer("Wolfgang von", "Trips", "Germany", new DateOnly(1928, 5, 4), new DateOnly(1961, 9, 10), Wins: 2),
        new Racer("Richie", "Ginther", "USA", new DateOnly(1930, 8, 5), new DateOnly(1989, 9, 20), Wins: 1),
        new Racer("Jackie", "Ickx", "Belgium", new DateOnly(1945, 1, 1), null, Wins: 8),
        new Racer("Clay", "Regazzoni", "Switzerland", new DateOnly(1939, 9, 5), new DateOnly(2006, 12, 15), Wins: 5),
        new Racer("Ronnie", "Peterson", "Sweden", new DateOnly(1944, 2, 14), new DateOnly(1978, 9, 11), Wins: 10),
        new Racer("Francois", "Cevert", "France", new DateOnly(1944, 2, 25), new DateOnly(1973, 10, 6), Wins: 1),
        new Racer("Carlos", "Reutemann", "Argentina", new DateOnly(1942, 4, 12), new DateOnly(2021, 7, 7), Wins: 12),
        new Racer("Gilles", "Villeneuve", "Canada", new DateOnly(1950, 1, 18), new DateOnly(1982, 5, 8), Wins: 6),
        new Racer("Didier", "Pironi", "France", new DateOnly(1952, 3, 26), new DateOnly(1987, 8, 23), Wins: 3),
        new Racer("John", "Watson", "UK", new DateOnly(1946, 5, 4), null, Wins: 5),
        new Racer("Rene", "Arnoux", "France", new DateOnly(1948, 7, 4), null, Wins: 7),
        new Racer("Elio", "de Angelis", "Italy", new DateOnly(1958, 3, 26), new DateOnly(1986, 5, 15), Wins: 2),
        new Racer("Michele", "Alboreto", "Italy", new DateOnly(1956, 12, 23), new DateOnly(2001, 4, 25), Wins: 5),
        new Racer("Gerhard", "Berger", "Austria", new DateOnly(1959, 8, 27), null, Wins: 10),
        new Racer("Riccardo", "Patrese", "Italy", new DateOnly(1954, 4, 17), null, Wins: 6),
        new Racer("David", "Coulthard", "UK", new DateOnly(1971, 3, 27), null, Wins: 13),
        new Racer("Heinz-Harald", "Frentzen", "Germany", new DateOnly(1967, 5, 18), null, Wins: 3),
        new Racer("Eddie", "Irvine", "UK", new DateOnly(1965, 11, 10), null, Wins: 4),
        new Racer("Rubens", "Barrichello", "Brazil", new DateOnly(1972, 5, 23), null, Wins: 11),
        new Racer("Juan Pablo", "Montoya", "Columbia", new DateOnly(1975, 9, 20), null, Wins: 7),
        new Racer("Felipe", "Massa", "Brazil", new DateOnly(1981, 4, 25), null, Wins: 11),
        new Racer("Mark", "Webber", "Australia", new DateOnly(1976, 8, 27), null, Wins: 9),
        new Racer("Daniel", "Ricciardo", "Australia", new DateOnly(1989, 7, 1), null, Wins: 8),
        new Racer("Valtteri", "Bottas", "Finland", new DateOnly(1989, 8, 28), null, Wins: 10),
        new Racer("Charles", "Leclerc", "Monaco", new DateOnly(1997, 10, 16), null, Wins: 8),
        new Racer("Sergio", "Perez", "Mexico", new DateOnly(1990, 1, 26), null, Wins: 6),
        new Racer("Lando", "Norris", "UK", new DateOnly(1999, 11, 13), null, Wins: 4)
    ];

    private static List<Championship>? s_championships;
    public static IEnumerable<Championship> GetChampionships() => s_championships ??=
    [
        new(1950, "Nino Farina", "Juan Manuel Fangio", "Luigi Fagioli"),
        new(1951, "Juan Manuel Fangio", "Alberto Ascari", "Froilan Gonzalez"),
        new(1952, "Alberto Ascari", "Nino Farina", "Piero Taruffi"),
        new(1953, "Alberto Ascari", "Juan Manuel Fangio", "Nino Farina"),
        new(1954, "Juan Manuel Fangio", "Froilan Gonzalez", "Mike Hawthorn"),
        new(1955, "Juan Manuel Fangio", "Stirling Moss", "Eugenio Castellotti"),
        new(1956, "Juan Manuel Fangio", "Stirling Moss", "Peter Collins"),
        new(1957, "Juan Manuel Fangio", "Stirling Moss", "Luigi Musso"),
        new(1958, "Mike Hawthorn", "Stirling Moss", "Tony Brooks"),
        new(1959, "Jack Brabham", "Tony Brooks", "Stirling Moss"),
        new(1960, "Jack Brabham", "Bruce McLaren", "Stirling Moss"),
        new(1961, "Phil Hill", "Wolfgang von Trips", "Stirling Moss"),
        new(1962, "Graham Hill", "Jim Clark", "Bruce McLaren"),
        new(1963, "Jim Clark", "Graham Hill", "Richie Ginther"),
        new(1964, "John Surtees", "Graham Hill", "Jim Clark"),
        new(1965, "Jim Clark", "Graham Hill", "Jackie Stewart"),
        new(1966, "Jack Brabham", "John Surtees", "Jochen Rindt"),
        new(1967, "Denny Hulme", "Jack Brabham", "Jim Clark"),
        new(1968, "Graham Hill", "Jackie Stewart", "Denny Hulme"),
        new(1969, "Jackie Stewart", "Jackie Ickx", "Bruce McLaren"),
        new(1970, "Jochen Rindt", "Jackie Ickx", "Clay Regazzoni"),
        new(1971, "Jackie Stewart", "Ronnie Peterson", "Francois Cevert"),
        new(1972, "Emerson Fittipaldi", "Jackie Stewart", "Denny Hulme"),
        new(1973, "Jackie Stewart", "Emerson Fittipaldi", "Ronnie Peterson"),
        new(1974, "Emerson Fittipaldi", "Clay Regazzoni", "Jody Scheckter"),
        new(1975, "Niki Lauda", "Emerson Fittipaldi", "Carlos Reutemann"),
        new(1976, "James Hunt", "Niki Lauda", "Jody Scheckter"),
        new(1977, "Niki Lauda", "Jody Scheckter", "Mario Andretti"),
        new(1978, "Mario Andretti", "Ronnie Peterson", "Carlos Reutemann"),
        new(1979, "Jody Scheckter", "Gilles Villeneuve", "Alan Jones"),
        new(1980, "Alan Jones", "Nelson Piquet", "Carlos Reutemann"),
        new(1981, "Nelson Piquet", "Carlos Reutemann", "Alan Jones"),
        new(1982, "Keke Rosberg", "Didier Pironi", "John Watson"),
        new(1983, "Nelson Piquet", "Alain Prost", "Rene Arnoux"),
        new(1984, "Niki Lauda", "Alain Prost", "Elio de Angelis"),
        new(1985, "Alain Prost", "Michele Alboreto", "Keke Rosberg"),
        new(1986, "Alain Prost", "Nigel Mansell", "Nelson Piquet"),
        new(1987, "Nelson Piquet", "Nigel Mansell", "Ayrton Senna"),
        new(1988, "Ayrton Senna", "Alain Prost", "Gerhard Berger"),
        new(1989, "Alain Prost", "Ayrton Senna", "Riccardo Patrese"),
        new(1990, "Ayrton Senna", "Alain Prost", "Nelson Piquet"),
        new(1991, "Ayrton Senna", "Nigel Mansell", "Riccardo Patrese"),
        new(1992, "Nigel Mansell", "Riccardo Patrese", "Michael Schumacher"),
        new(1993, "Alain Prost", "Ayrton Senna", "Damon Hill"),
        new(1994, "Michael Schumacher", "Damon Hill", "Gerhard Berger"),
        new(1995, "Michael Schumacher", "Damon Hill", "David Coulthard"),
        new(1996, "Damon Hill", "Jacques Villeneuve", "Michael Schumacher"),
        new(1997, "Jacques Villeneuve", "Heinz-Harald Frentzen", "David Coulthard"),
        new(1998, "Mika Häkkinen", "Michael Schumacher", "David Coulthard"),
        new(1999, "Mika Häkkinen", "Eddie Irvine", "Heinz-Harald Frentzen"),
        new(2000, "Michael Schumacher", "Mika Häkkinen", "David Coulthard"),
        new(2001, "Michael Schumacher", "David Coulthard", "Rubens Barrichello"),
        new(2002, "Michael Schumacher", "Rubens Barrichello", "Juan Pablo Montoya"),
        new(2003, "Michael Schumacher", "Kimi Räikkönen", "Juan Pablo Montoya"),
        new(2004, "Michael Schumacher", "Rubens Barrichello", "Jenson Button"),
        new(2005, "Fernando Alonso", "Kimi Räikkönen", "Michael Schumacher"),
        new(2006, "Fernando Alonso", "Michael Schumacher", "Felipe Massa"),
        new(2007, "Kimi Räikkönen", "Lewis Hamilton", "Fernando Alonso"),
        new(2008, "Lewis Hamilton", "Felipe Massa", "Kimi Räikkönen"),
        new(2009, "Jenson Button", "Sebastian Vettel", "Rubens Barrichello"),
        new(2010, "Sebastian Vettel", "Fernando Alonso", "Mark Webber"),
        new(2011, "Sebastian Vettel", "Jenson Button", "Mark Webber"),
        new(2012, "Sebastian Vettel", "Fernando Alonso", "Kimi Räikkönen"),
        new(2013, "Sebastian Vettel", "Fernando Alonso", "Mark Webber"),
        new(2014, "Lewis Hamilton", "Nico Rosberg", "Daniel Ricciardo"),
        new(2015, "Lewis Hamilton", "Nico Rosberg", "Sebastian Vettel"),
        new(2016, "Nico Rosberg", "Lewis Hamilton", "Daniel Ricciardo"),
        new(2017, "Lewis Hamilton", "Sebastian Vettel", "Valtteri Bottas"),
        new(2018, "Lewis Hamilton", "Sebastian Vettel", "Kimi Räikkönen"),
        new(2019, "Lewis Hamilton", "Valtteri Bottas", "Max Verstappen"),
        new(2020, "Lewis Hamilton", "Valtteri Bottas", "Max Verstappen"),
        new(2021, "Max Verstappen", "Lewis Hamilton", "Valtteri Bottas"),
        new(2022, "Max Verstappen", "Charles Leclerc","Sergio Perez"),
        new(2023, "Max Verstappen", "Sergio Perez", "Lewis Hamilton"),
        new(2024, "Max Verstappen", "Lando Norris", "Charles Leclerc")
    ];
}
