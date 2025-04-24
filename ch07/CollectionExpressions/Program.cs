// array initialized with collection expressions
int[] numbers1 = [1, 2, 3, 4, 5];

// a List initialized with collection expressions
List<string> words = ["one", "two", "three", "four"];

// a variable of an interface type initialized with collection expressions
IEnumerable<Person> people =[new Person("John", "Doe"), new Person("Jane", "Doe")];

// using the spread element ..
List<int> numbers2 = [6, 7, 8, 9];
List<int> combined = [.. numbers1, .. numbers2];
ShowItems(nameof(combined), combined); // 1, 2, 3, 4, 5, 6, 7, 8, 9

// using the spread element with the range operator and indices
List<int> numbers3 = [.. numbers1[2..^1]]; // 3, 4 select a range from the third element up to excluding the last element
int x = numbers1[^1]; // select the last element using indices

// the following two statements represent the same range, implicit and explicit.
var r1 = 2..^1;
Range r2 = new (new Index(2), new Index(1, fromEnd: true));
List<int> numbers4 = [.. numbers1[r2]];

ShowItems(nameof(numbers3), numbers3);

// using a custom type with collection expressions
CustomCollection<string> customCollection = 
    ["one", "two", "three", "four", "five", "six"];

CustomCollection<string> slice = [.. customCollection[2..4]];

ShowItems(nameof(CustomCollection), customCollection);

static void ShowItems<T>(string title, IEnumerable<T> items)
{
    Console.WriteLine(title);
    foreach (T item in items)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine();
}
