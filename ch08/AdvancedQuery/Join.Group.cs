using DataLib;

namespace AdvancedQuery;
internal partial class Join
{
    /// <summary>
    /// Demonstrates how to perform a group join operation using LINQ to correlate Formula 1 champions with their
    /// championship results across multiple years.
    /// </summary>
    /// <remarks>This method retrieves Formula 1 champions and their respective championship results, 
    /// grouping the results by each champion. The output includes the champion's name, total wins,  and a list of their
    /// positions in various championship years.  The method uses LINQ to join two data sources: a list of Formula 1
    /// champions and a list of  championship results. The join is performed based on the champion's first and last
    /// names.</remarks>
    public static void GroupJoin()
    {
        Console.WriteLine("Group Join");
        var racers = from cs in Formula1.GetChampionships()
                     from r in new List<(int Year, int Position, string FirstName, string LastName)>()
                         {
                             (cs.Year, Position: 1, FirstName: cs.First.FirstName(), LastName: cs.First.LastName()),
                             (cs.Year, Position: 2, FirstName: cs.Second.FirstName(), LastName: cs.Second.LastName()),
                             (cs.Year, Position: 3, FirstName: cs.Third.FirstName(), LastName: cs.Third.LastName())
                         }
                     select r;

        var q = (from r in Formula1.GetFormula1Champions()
                 join r2 in racers on
                 (
                     r.FirstName,
                     r.LastName
                 )
                 equals
                 (
                     r2.FirstName,
                     r2.LastName
                 )
                 into yearResults
                 select
                 (
                     r.FirstName,
                     r.LastName,
                     r.Wins,
                     Results: yearResults
                 ));

        foreach (var r in q)
        {
            Console.WriteLine($"{r.FirstName} {r.LastName}");
            foreach (var results in r.Results)
            {
                Console.WriteLine($"\t{results.Year} {results.Position}");
            }
        }
        Console.WriteLine();
    }

    public static void GroupJoinWithMethods()
    {
        var racers = Formula1.GetChampionships()
          .SelectMany(cs => new List<(int Year, int Position, string FirstName, string LastName)>
          {
                 (cs.Year, Position: 1, FirstName: cs.First.FirstName(), LastName: cs.First.LastName()),
                 (cs.Year, Position: 2, FirstName: cs.Second.FirstName(), LastName: cs.Second.LastName()),
                 (cs.Year, Position: 3, FirstName: cs.Third.FirstName(), LastName: cs.Third.LastName())
          });

        var q = Formula1.GetFormula1Champions()
            .GroupJoin(racers,
                r1 => (r1.FirstName, r1.LastName),
                r2 => (r2.FirstName, r2.LastName),
                (r1, r2s) => (r1.FirstName, r1.LastName, r1.Wins, Results: r2s));


        foreach (var r in q)
        {
            Console.WriteLine($"{r.FirstName} {r.LastName}");
            foreach (var results in r.Results)
            {
                Console.WriteLine($"{results.Year} {results.Position}");
            }
        }
    }
}
