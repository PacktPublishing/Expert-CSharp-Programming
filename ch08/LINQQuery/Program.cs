using DataLib;

var champions = Formula1.GetFormula1Champions();

LinqQuerySyntax(champions);
LinqMethodSyntax(champions);

static void LinqQuerySyntax(IEnumerable<Racer> champions)
{
    var q = 
        from r in champions
            let numberChampionShips = r.Championships.Count()
            where numberChampionShips > 3
            orderby numberChampionShips descending, r.LastName ascending
            select new { r.FirstName, r.LastName, Championships = numberChampionShips };

    foreach (var racer in q)
    {
        Console.WriteLine($"{racer.FirstName} {racer.LastName} - {racer.Championships} Championships");
    }
    Console.WriteLine();
}

static void LinqMethodSyntax(IEnumerable<Racer> champions)
{
    var q = champions
        .Select(r => new { Racer = r, NumberChampionships = r.Championships.Count() })
        .Where(x => x.NumberChampionships > 3)
        .OrderByDescending(x => x.NumberChampionships)
        .ThenBy(x => x.Racer.LastName)
        .Select(x => new { x.Racer.FirstName, x.Racer.LastName, Championships = x.NumberChampionships });
    foreach (var item in q)
    {
        Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Championships} Championships");
    }
    Console.WriteLine();
}
