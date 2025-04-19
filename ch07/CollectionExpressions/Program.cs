int[] numbers1 = [1, 2, 3, 4, 5];
List<string> words = ["one", "two", "three", "four"];
IEnumerable<Person> people =[new Person("John", "Doe"), new Person("Jane", "Doe")];

List<int> numbers2 = [6, 7, 8, 9];
List<int> combined = [.. numbers1, .. numbers2];
ShowItems(nameof(combined), combined); // 1, 2, 3, 4, 5, 6, 7, 8, 9

List<int> numbers3 = [.. numbers1[2..^1]]; // 3, 4
int x = numbers1[^1]; // select the last element

ShowItems(nameof(numbers3), numbers3);

CustomCollection<string> customCollection = ["one", "two", "three"];

ShowItems(nameof(CustomCollection), customCollection);

void ShowItems<T>(string title, IEnumerable<T> items)
{
    Console.WriteLine(title);
    foreach (T item in items)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine();
}
