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
    Console.WriteLine(ex);
}

try
{
    var filteredData2 = CustomEnumerator.Filter2<int>(null!, null!);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}


