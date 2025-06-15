using DataLib;

namespace AdvancedQuery;

internal class Grouping
{
    /// <summary>
    /// Displays a list of Formula 1 champions grouped by their country, using a LINQ query.
    /// </summary>
    /// <remarks>This method retrieves Formula 1 champions, groups them by their country, and filters the
    /// results to include only countries with at least two champions. The output is sorted by the number of champions
    /// in descending order, and then alphabetically by country name. Within each country, the champions are listed in
    /// alphabetical order by their last name. The method outputs the results to the console, showing the country name,
    /// the number of champions, and the full names of the champions.</remarks>
    public static void ShowChampionsByCountryLINQQuery()
    {
        Console.WriteLine("Grouping - LINQ Query");

        var countries = 
            from r in Formula1.GetFormula1Champions()
            group r by r.Country into g
            let count = g.Count()
            where count >= 2
            orderby count descending, g.Key
            select 
            (
                Country: g.Key,
                Count: count,
                Racers: from r1 in g
                        orderby r1.LastName
                        select r1.FirstName + " " + r1.LastName
            );

        foreach ((string country, int numberChampions, IEnumerable<string> racers) in countries)
        {
            Console.WriteLine($"{country} - {numberChampions} Champions");
            foreach (var racer in racers)
            {
                Console.WriteLine($" {racer}");
            }
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Displays a list of Formula 1 champion racers grouped by their country.
    /// </summary>
    /// <remarks>This method retrieves Formula 1 champion data, groups the racers by their country, and
    /// filters the results to include only countries with at least two champions. The output is sorted by the number of
    /// champions in descending order, followed by the country name in ascending order. Within each country group,
    /// racers are listed alphabetically by their last name.</remarks>
    public static void ShowChampionsByCountry()
    {
        Console.WriteLine("Grouping Method Syntax");

        var countries = Formula1.GetFormula1Champions()
            .GroupBy(r => r.Country)
            .Select(g => new { Group = g, Country = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ThenBy(g => g.Country)
            .Where(g => g.Count >= 2)
            .Select(g => 
            (
                g.Country,
                g.Count,
                Racers: g.Group.OrderBy(r => r.LastName).Select(r => $"{r.FirstName} {r.LastName}")
            ));

        foreach (var (Country, Count, Racers) in countries)
        {
            Console.WriteLine($"{Country} - {Count} Champions");
            foreach (var racer in Racers)
            {
                Console.WriteLine($"  {racer}");
            }
        }
        Console.WriteLine();
    }
}
