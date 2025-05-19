namespace DataLib;
public class Formula1
{
    public static IEnumerable<Racer> GetFormula1Champions()
    {
        List<Racer> champions = [
        // 1950s champions...
        new("Giuseppe", "Farina", "Italy", "Alfa Romeo", new DateOnly(1906, 10, 30), new DateOnly(1966, 6, 30))
        {
            Wins = 5,
            PolePositions = 5,
            Championships = [1950]
        },
        new("Juan Manuel", "Fangio", "Argentina", "Alfa Romeo", new DateOnly(1911, 6, 24), new DateOnly(1995, 7, 17))
        {
            Wins = 24,
            PolePositions = 29,
            Championships = [1951, 1954, 1955, 1956, 1957]
        },
        new("Alberto", "Ascari", "Italy", "Ferrari", new DateOnly(1918, 7, 13), new DateOnly(1955, 5, 26))
        {
            Wins = 13,
            PolePositions = 14,
            Championships = [1952, 1953]
        },
        new("Mike", "Hawthorn", "United Kingdom", "Ferrari", new DateOnly(1929, 4, 10), new DateOnly(1959, 1, 22))
        {
            Wins = 3,
            PolePositions = 4,
            Championships = [1958]
        },
        new("Jack", "Brabham", "Australia", "Cooper", new DateOnly(1926, 4, 2), new DateOnly(2014, 5, 19))
        {
            Wins = 14,
            PolePositions = 13,
            Championships = [1959, 1960]
        },
        // 1960s champions...
        new("Phil", "Hill", "United States", "Ferrari", new DateOnly(1927, 4, 20), new DateOnly(2008, 8, 28))
        {
            Wins = 3,
            PolePositions = 6,
            Championships = [1961]
        },
        new("Graham", "Hill", "United Kingdom", "BRM", new DateOnly(1929, 2, 15), new DateOnly(1975, 11, 29))
        {
            Wins = 14,
            PolePositions = 13,
            Championships = [1962, 1968]
        },
        new("Jim", "Clark", "United Kingdom", "Lotus", new DateOnly(1936, 3, 4), new DateOnly(1968, 4, 7))
        {
            Wins = 25,
            PolePositions = 33,
            Championships = [1963, 1965]
        },
        new("John", "Surtees", "United Kingdom", "Ferrari", new DateOnly(1934, 2, 11), new DateOnly(2017, 3, 10))
        {
            Wins = 6,
            PolePositions = 8,
            Championships = [1964]
        },
        new("Jack", "Brabham", "Australia", "Brabham", new DateOnly(1926, 4, 2), new DateOnly(2014, 5, 19))
        {
            Wins = 14,
            PolePositions = 13,
            Championships = [1959, 1960, 1966]
        },
        new("Denny", "Hulme", "New Zealand", "Brabham", new DateOnly(1936, 6, 18), new DateOnly(1992, 10, 4))
        {
            Wins = 8,
            PolePositions = 1,
            Championships = [1967]
        },
        new("Jackie", "Stewart", "United Kingdom", "Matra", new DateOnly(1939, 6, 11))
        {
            Wins = 27,
            PolePositions = 17,
            Championships = [1969, 1971, 1973]
        },
        new("Jochen", "Rindt", "Austria", "Lotus", new DateOnly(1942, 4, 18), new DateOnly(1970, 9, 5))
        {
            Wins = 6,
            PolePositions = 10,
            Championships = [1970]
        },
        // 1970s champions
        new("Jackie", "Stewart", "United Kingdom", "Tyrrell", new DateOnly(1939, 6, 11))
        {
            Wins = 27,
            PolePositions = 17,
            Championships = [1969, 1971, 1973]
        },
        new("Emerson", "Fittipaldi", "Brazil", "Lotus", new DateOnly(1946, 12, 12))
        {
            Wins = 14,
            PolePositions = 6,
            Championships = [1972, 1974]
        },
        new("Niki", "Lauda", "Austria", "Ferrari", new DateOnly(1949, 2, 22), new DateOnly(2019, 5, 20))
        {
            Wins = 25,
            PolePositions = 24,
            Championships = [1975, 1977, 1984]
        },
        new("James", "Hunt", "United Kingdom", "McLaren", new DateOnly(1947, 8, 29), new DateOnly(1993, 6, 15))
        {
            Wins = 10,
            PolePositions = 14,
            Championships = [1976]
        },
        new("Mario", "Andretti", "United States", "Lotus", new DateOnly(1940, 2, 28))
        {
            Wins = 12,
            PolePositions = 18,
            Championships = [1978]
        },
        new("Jody", "Scheckter", "South Africa", "Ferrari", new DateOnly(1950, 1, 29))
        {
            Wins = 10,
            PolePositions = 3,
            Championships = [1979]
        },
        new("Alan", "Jones", "Australia", "Williams", new DateOnly(1946, 11, 2))
        {
            Wins = 12,
            PolePositions = 6,
            Championships = [1980]
        },
        // After Alan Jones...
        new("Nelson", "Piquet", "Brazil", "Brabham", new DateOnly(1952, 8, 17))
        {
            Wins = 23,
            PolePositions = 24,
            Championships = [1981, 1983, 1987]
        },
        new("Keke", "Rosberg", "Finland", "Williams", new DateOnly(1948, 12, 6))
        {
            Wins = 5,
            PolePositions = 5,
            Championships = [1982]
        },
        new("Alain", "Prost", "France", "McLaren", new DateOnly(1955, 2, 24))
        {
            Wins = 51,
            PolePositions = 33,
            Championships = [1985, 1986, 1989, 1993]
        },
        // After Alain Prost...
        new("Nigel", "Mansell", "United Kingdom", "Williams", new DateOnly(1953, 8, 8))
        {
            Wins = 31,
            PolePositions = 32,
            Championships = [1992]
        },
        new("Damon", "Hill", "United Kingdom", "Williams", new DateOnly(1960, 9, 17))
        {
            Wins = 22,
            PolePositions = 20,
            Championships = [1996]
        },
        new("Jacques", "Villeneuve", "Canada", "Williams", new DateOnly(1971, 4, 9))
        {
            Wins = 11,
            PolePositions = 13,
            Championships = [1997]
        },
        new("Mika", "Häkkinen", "Finland", "McLaren", new DateOnly(1968, 9, 28))
        {
            Wins = 20,
            PolePositions = 26,
            Championships = [1998, 1999]
        },
        // After Mika Häkkinen and before existing modern champions...
        new("Fernando", "Alonso", "Spain", "Renault", new DateOnly(1981, 7, 29))
        {
            Wins = 32,
            PolePositions = 22,
            Championships = [2005, 2006]
        },
        new("Kimi", "Räikkönen", "Finland", "Ferrari", new DateOnly(1979, 10, 17))
        {
            Wins = 21,
            PolePositions = 18,
            Championships = [2007]
        },
        new("Jenson", "Button", "United Kingdom", "Brawn GP", new DateOnly(1980, 1, 19))
        {
            Wins = 15,
            PolePositions = 8,
            Championships = [2009]
        },
        new("Sebastian", "Vettel", "Germany", "Red Bull Racing", new DateOnly(1987, 7, 3))
        {
            Wins = 53,
            PolePositions = 57,
            Championships = [2010, 2011, 2012, 2013]
        },
        new("Nico", "Rosberg", "Germany", "Mercedes", new DateOnly(1985, 6, 27))
        {
            Wins = 23,
            PolePositions = 30,
            Championships = [2016]
        },
        // modern champions...
        new("Lewis", "Hamilton", "United Kingdom", "Mercedes", new DateOnly(1985, 1, 7))
        {
            Wins = 103,
            PolePositions = 104,
            Championships = [2008, 2014, 2015, 2017, 2018, 2019, 2020]
        },
        new("Michael", "Schumacher", "Germany", "Ferrari", new DateOnly(1969, 1, 3))
        {
            Wins = 91,
            PolePositions = 68,
            Championships = [1994, 1995, 2000, 2001, 2002, 2003, 2004]
        },
        new("Ayrton", "Senna", "Brazil", "McLaren", new DateOnly(1960, 3, 21), new DateOnly(1994, 5, 1))
        {
            Wins = 41,
            PolePositions = 65,
            Championships = [1988, 1990, 1991]
        },
        new("Max", "Verstappen", "Netherlands", "Red Bull Racing", new DateOnly(1997, 9, 30))
        {
            Wins = 54,
            PolePositions = 32,
            Championships = [2021, 2022, 2023]
        }];

        return champions;
    }
}
