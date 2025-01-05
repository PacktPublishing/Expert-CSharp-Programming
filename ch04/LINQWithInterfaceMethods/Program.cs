using LINQWithInterfaceMethods;

IEnumerableEx<int> intValues = [1, 3, 4, 5, 6, 8, 11, 33];

CustomDictionary<int, string> dict = new() 
{ 
    [1] = "one",
    [2] = "two",
    [3] = "three",
    [4] = "four",
    [5] = "five"
};

CachedList<int> cachedList = [1, 3, 4, 5, 6, 8, 11, 33];

ShowEvenValues("IEnumerableEx<int>", intValues);
ShowEvenValues("CachedList<int>", cachedList);
cachedList.Add(44);
ShowEvenValues("CachedList<int> 2", cachedList);
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
