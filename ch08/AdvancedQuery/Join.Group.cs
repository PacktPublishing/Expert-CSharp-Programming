using DataLib;

namespace AdvancedQuery;
internal partial class Join
{
    /// <summary>
    /// Demonstrates an inner join operation using LINQ to combine Formula 1 driver and constructor championship data
    /// based on the same year.
    /// </summary>
    /// <remarks>This method retrieves Formula 1 driver and constructor championship data, performs an inner
    /// join on the championship year,  and displays the top 10 results ordered by year. The output includes the year,
    /// the driver's name, and the constructor's name.</remarks>
    public static void InnerJoin()
    {
        Console.WriteLine("Inner join with a LINQ query - combining driver and constructor championship based on the same year");

        var racers = from r in Formula1.GetFormula1Champions()
                     from y in r.Championships
                     select (
                         Year: y,
                         Name: r.FirstName + " " + r.LastName
                     );

        var teams = from t in Formula1.GetConstructorChampions()
                    from y in t.Championships
                    select (                    
                        Year: y,
                        t.Name
                    );

        var racersAndTeams =
              (from r in racers
               join t in teams on r.Year equals t.Year
               orderby t.Year
               select new
               {
                   r.Year,
                   Champion = r.Name,
                   Constructor = t.Name
               }).Take(10);

        Console.WriteLine("Year  World Champion\t   Constructor Title");
        foreach (var item in racersAndTeams)
        {
            Console.WriteLine($"{item.Year}: {item.Champion,-20} {item.Constructor}");
        }
        Console.WriteLine();
    }

    public static void InnerJoinWithMethods()
    {
        Console.WriteLine("Inner join with method syntax - combining driver and constructor championship based on the same year");

        var racers = Formula1.GetFormula1Champions()
            .SelectMany(r => r.Championships, (r1, year) =>
            new
            {
                Year = year,
                Name = $"{r1.FirstName} {r1.LastName}"
            });

        var teams = Formula1.GetConstructorChampions()
            .SelectMany(t => t.Championships, (t, year) =>
            new
            {
                Year = year,
                t.Name
            });

        var racersAndTeams = racers.Join(
            teams,
            r => r.Year,
            t => t.Year,
            (r, t) =>
                new
                {
                    r.Year,
                    Champion = r.Name,
                    Constructor = t.Name
                }).OrderBy(item => item.Year)
                .Take(10);

        Console.WriteLine("Year  Driver\t\t   Constructor");
        foreach (var item in racersAndTeams)
        {
            Console.WriteLine($"{item.Year}: {item.Champion,-20} {item.Constructor}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Performs a left outer join between Formula 1 driver champions and constructor champions,  displaying the top 10
    /// results ordered by year.
    /// </summary>
    /// <remarks>This method retrieves data about Formula 1 driver champions and constructor champions, 
    /// performs a left outer join based on the championship year, and outputs the results to the console. If no
    /// constructor championship exists for a given year, the result will indicate "no constructor
    /// championship."</remarks>
    public static void LeftOuterJoin()
    {
        var racers = from r in Formula1.GetFormula1Champions()
                     from y in r.Championships
                     select new
                     {
                         Year = y,
                         Name = r.FirstName + " " + r.LastName
                     };

        var teams = from t in Formula1.GetConstructorChampions()
                    from y in t.Championships
                    select new
                    {
                        Year = y,
                        t.Name
                    };

        var racersAndTeams =
          (from r in racers
           join t in teams on r.Year equals t.Year into ts
           from t in ts.DefaultIfEmpty()
           orderby r.Year
           select new
           {
               r.Year,
               Champion = r.Name,
               Constructor = t == null ? "no constructor championship" : t.Name
           }).Take(10);

        Console.WriteLine("Year  Champion\t\t   Constructor Title");
        foreach (var item in racersAndTeams)
        {
            Console.WriteLine($"{item.Year}: {item.Champion,-20} {item.Constructor}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates a left outer join operation using LINQ methods to combine Formula 1 driver and constructor
    /// championship data.
    /// </summary>
    /// <remarks>This method retrieves Formula 1 driver and constructor championship data, performs a left
    /// outer join based on the championship year,  and outputs the results to the console. The output includes the
    /// year, the driver's name, and the constructor's name, or a placeholder  if no constructor championship exists for
    /// that year. <para> The method uses LINQ operations such as <see cref="Enumerable.GroupJoin"/> and <see
    /// cref="Enumerable.DefaultIfEmpty"/> to perform the join. </para></remarks>
    public static void LeftOuterJoinWithMethods()
    {
        var racers = Formula1.GetFormula1Champions()
            .SelectMany(r => r.Championships, (r1, year) =>
            new
            {
                Year = year,
                Name = $"{r1.FirstName} {r1.LastName}"
            });

        var teams = Formula1.GetConstructorChampions()
            .SelectMany(t => t.Championships, (t, year) =>
            new
            {
                Year = year,
                t.Name
            });

        var racersAndTeams =
            racers.GroupJoin(
                teams,
                r => r.Year,
                t => t.Year,
                (r, ts) => new
                {
                    r.Year,
                    Champion = r.Name,
                    Constructors = ts
                })
                .SelectMany(
                    item => item.Constructors.DefaultIfEmpty(),
                    (r, t) => new
                    {
                        r.Year,
                        r.Champion,
                        Constructor = t?.Name ?? "no constructor championship"
                    });

        Console.WriteLine("Year  Driver\t\t   Constructor");
        foreach (var item in racersAndTeams)
        {
            Console.WriteLine($"{item.Year}: {item.Champion,-20} {item.Constructor}");
        }
        Console.WriteLine();
    }

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
