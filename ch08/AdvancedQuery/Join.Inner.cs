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
   
}
