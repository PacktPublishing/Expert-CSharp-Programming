using DataLib;

namespace AdvancedQuery;

internal class CompoundFrom
{

    /// <summary>
    /// Displays a list of Formula 1 champions who have raced for the specified team,  ordered by their number of wins
    /// in descending order and then by last name.
    /// </summary>
    /// <remarks>This method queries a collection of Formula 1 champions and their associated teams to find
    /// all champions who have raced for the specified team. The results are displayed  in the format "FirstName
    /// LastName - Wins Wins".</remarks>
    /// <param name="team">The name of the team to filter champions by. Cannot be null or empty.</param>
    public static void CompoundFromLINQQuery(string team)
    {
        Console.WriteLine($"Compound From - LINQ Query, champions with {team}");

        var racers = 
            from r in Formula1.GetFormula1Champions()
            from t in r.Teams
            where t.Name == team
            orderby r.Wins descending, r.LastName
            select $"{r.FirstName} {r.LastName} - {r.Wins} Wins";

        foreach (var racer in racers)
        {
            Console.WriteLine(racer);
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Displays a list of Formula 1 champions who raced for the specified team, ordered by their number of wins and
    /// last name.
    /// </summary>
    /// <remarks>This method retrieves Formula 1 champions and filters them based on the specified team. The
    /// results are ordered by the number of wins in descending order and then by last name in ascending order. Each
    /// champion's name and win count are displayed in the console.</remarks>
    /// <param name="team">The name of the team to filter the Formula 1 champions by. Cannot be null or empty.</param>
    public static void CompoundFromMethodSyntax(string team)
    {
        Console.WriteLine($"Compound From - LINQ Query, champions with {team}");

        var racers = Formula1.GetFormula1Champions()
            .SelectMany(r => r.Teams, (r, t) => new { Racer = r, Team = t })
            .Where(r => r.Team.Name == team)
            .OrderByDescending(r => r.Racer.Wins)
            .ThenBy(r => r.Racer.LastName)
            .Select(r => $"{r.Racer.FirstName} {r.Racer.LastName} - {r.Racer.Wins} Wins");

        foreach (var racer in racers)
        {
            Console.WriteLine(racer);
        }
        Console.WriteLine();
    }
}
