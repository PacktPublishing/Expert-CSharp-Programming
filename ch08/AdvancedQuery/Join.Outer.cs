using DataLib;

namespace AdvancedQuery;
internal partial class Join
{
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
}
