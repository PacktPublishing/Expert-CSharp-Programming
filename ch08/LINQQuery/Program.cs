using DataLib;

var champions = Formula1.GetFormula1Champions();

//LinqQuerySyntax(champions);
//LinqMethodSyntax(champions);
// UsingCustomMethod(champions);
UsingCustomMethodPassingNull(champions);
UsingCustomMethodPassingNullWithEarlyValidation(champions);

static void LinqQuerySyntax(IEnumerable<Racer> champions)
{
    var q = from r in champions
            where r.Championships.Count > 3
            orderby r.Championships.Count descending, r.LastName ascending
            select new { r.FirstName, r.LastName, Championships = r.Championships.Count };

    foreach (var item in q)
    {
        Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Championships} Championships");
    }
    Console.WriteLine();
}

static void LinqMethodSyntax(IEnumerable<Racer> champions)
{
    var q = champions
        .Where(r => r.Championships.Count > 3)
        .OrderByDescending(r => r.Championships.Count)
        .ThenBy(r => r.LastName)
        .Select(r => new { r.FirstName, r.LastName, Championships = r.Championships.Count });
    foreach (var item in q)
    {
        Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Championships} Championships");
    }
    Console.WriteLine();
}

static void UsingCustomMethod(IEnumerable<Racer> champions)
{
    var q = champions.Filter(r => r.Championships.Count > 6);

    foreach (var item in q)
    {
        Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Championships} Championships");
    }
    Console.WriteLine();
}

static IEnumerable<Racer> GetNull() => null!;

static void UsingCustomMethodPassingNull(IEnumerable<Racer> champions)
{
    IEnumerable<Racer> racers = GetNull();
    var result = racers.Filter(r => r.LastName.StartsWith("Sen")); // This will throw an exception

    foreach (var item in result)
    {
        Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Championships} Championships");
    }

    Console.WriteLine();
}


static void UsingCustomMethodPassingNullWithEarlyValidation(IEnumerable<Racer> champions)
{
    IEnumerable<Racer> racers = GetNull();
    var result = racers!.Filter1(r => r.LastName.StartsWith("Sen")); // This will throw an exception

    foreach (var item in result)
    {
        Console.WriteLine($"{item.FirstName} {item.LastName} - {item.Championships} Championships");
    }

    Console.WriteLine();
}

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<T> Filter1(Func<T, bool> predicate)
        {
            static void ValidateArguments(IEnumerable<T> source, Func<T, bool> predicate)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(predicate);
            }

            ValidateArguments(source, predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}

public static class OlderExtensions
{
    // This is an older version of the extension method, which is not recommended for use in modern C#.
    // It uses a different syntax and does not support nullable reference types.
    // It's included here for educational purposes only.
    [Obsolete("This method uses older syntax. Use the newer version instead.")]
    public static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}
