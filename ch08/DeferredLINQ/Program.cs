using CustomImplementation;

int[] data = [1, 2, 3, 4, 5, 6, 7, 8, 9];

var filteredData = CustomEnumerator.Filter1<int>(null!, null!);

try
{
    foreach (var item in filteredData)
    {
        Console.Write($"{item} ");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{
    var filteredData2 = CustomEnumerator.Filter2<int>(null!, null!);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

List<string> names = ["John", "Niki", "Max", "Jochen", "Jack", "Lewis", "Juan"];

var query = names.Where(
    name => name.StartsWith('J'))
    .OrderBy(n => n);

ShowNames("1st iteration", query);

names.AddRange("Jenson", "Emerson", "Jody");

ShowNames("2nd iteration", query);
static void ShowNames(string title, IEnumerable<string> names)
{
    Console.WriteLine(title);
    foreach (var name in names)
    {
        Console.Write($"{name} ");
    }
    Console.WriteLine();
}
