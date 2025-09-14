using Traits;

IEnumerableEx<int> intValues = [1, 3, 4, 5, 6, 8, 11, 33];

CustomDictionary<int, string> dict = new() 
{ 
    [1] = "one",
    [2] = "two",
    [3] = "three",
    [4] = "four",
    [5] = "five"
};

SnapshotList<int> snapshotList = [1, 3, 4, 5, 6, 8, 11, 33];

ShowEvenValues("IEnumerableEx<int>", intValues);
ShowEvenValues("SnapshotList<int>", snapshotList);
snapshotList.Add(44);
Console.WriteLine("As the snapshot was remembered, 44 is not shown: ");
ShowEvenValues("SnapshotList<int> 2", snapshotList);
ShowEvenDictionaryValues("CustomDictionary<int, string>", dict);

void ShowEvenValues(string title, IEnumerableEx<int> values)
{
    Console.WriteLine(title);
    foreach (var x in values.Where(x => x % 2 == 0))
    {
        Console.WriteLine(x);
    }
    Console.WriteLine();
}

void ShowEvenDictionaryValues(string title, IEnumerableEx<KeyValuePair<int, string>> values)
{
    Console.WriteLine(title);
    foreach (var x in values.Where(x => x.Key % 2 == 0))
    {
        Console.WriteLine(x);
    }
    Console.WriteLine();
}
