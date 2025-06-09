using CustomImplementation;

IEnumerable<int> data = [1, 2, 3, 4, 5, 6, 7, 8, 9];

Console.WriteLine("Iterating through all elements");
foreach (var item in data)
{
    Console.Write($"{item} ");
}
Console.WriteLine();

Console.WriteLine("Using while method");
using IEnumerator<int> enumerator = data.GetEnumerator();
while (enumerator.MoveNext())
{
    Console.Write($"{enumerator.Current} ");
}
Console.WriteLine();


Console.WriteLine("Using Filter method with yield");
foreach (int item in Filter(data, x => x % 2 == 0))
{
    Console.Write($"{item} ");
}
Console.WriteLine();

Console.WriteLine("Using FilterEnumerable class ");
foreach (var item in new FilterEnumerable<int>(data, x => x % 2 == 0))
{
    Console.Write($"{item} ");
}
Console.WriteLine();

static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> predicate)
{
    foreach (var item in source)
    {
        if (predicate(item))
        {
            yield return item;
        }
    }
}
