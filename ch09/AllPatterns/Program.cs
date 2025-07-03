// Pattern Matching Examples in C# 13 / .NET 9

object value = 42;

// Type Pattern (since C# 7.0, declaration with C# 8)
if (value is int intValue1)
    Console.WriteLine($"Type pattern: {intValue1} is an int");

// Pattern Matching with Generic Types (since C# 7)
object genericObj = new List<int>() { 1, 2, 3 };
if (genericObj is List<int> intList)
    Console.WriteLine($"Generic type pattern: List<int> with count {intList.Count}");
else if (genericObj is List<string> stringList)
    Console.WriteLine($"Generic type pattern: List<string> with count {stringList.Count}");

// Constant Pattern (since C# 7.0)
if (value is 42)
    Console.WriteLine("Constant pattern: Value is 42");

// Var Pattern (since C# 7.0)
if (value is var v)
    Console.WriteLine($"var pattern: {v}");

// Relational Pattern (since C# 9.0)
if (value is int intValue2 && intValue2 > 40)
    Console.WriteLine("Relational pattern: Value is greater than 40");

// Logical Patterns (and, or, not) (since C# 9.0)
int test = 15;
if (test is > 10 and < 20)
    Console.WriteLine($"10. Logical pattern: {test} is between 10 and 20");
if (test is < 10 or > 20)
    Console.WriteLine($"10. Logical pattern: {test} is less than 10 or greater than 20");
if (test is not 0)
    Console.WriteLine($"10. Logical pattern: {test} is not zero");

// Parenthesized Pattern (since C# 9.0)
int parenthesizedTest = 101;
if (parenthesizedTest is (< 0 or > 10) and not 100)
    Console.WriteLine($"Parenthesized pattern: {parenthesizedTest} is less than 0 or greater than 10, and not 100");

// Property Pattern (since C# 8.0, enhancements in C# 9.0+)
var people = GetPeople();
foreach (var person in people)
{
    if (person is { FirstName: "Clark"})
    {
        Console.WriteLine($"Property pattern: {person} is a Clark");
    }
    if (person is { Address: { City: "Smallville"} })
    {
        Console.WriteLine($"Recursive property pattern: {person} lives in Smallville");
    }
    if (person is { Address.City: "Gotham City" })
    {
        Console.WriteLine($"Dot-separated recursive property pattern: {person} lives in Gotham City");
    }
}

// Positional Pattern (since C# 8.0 deconstruction for tuples and records, C# 9.0 for positional pattern matching)
Point point = new(3, 4);
if (point is (3, 4))
    Console.WriteLine("Positional pattern: Point is (3, 4)");

// 8. Discard Pattern (since C# 7.0, expanded in later versions)
var tuple = (42, "hello", true);
if (tuple is (42, _, _))
    Console.WriteLine("Discard pattern: First item is 42, others are ignored");

// 9. List Pattern (since C# 12.0)
int[] numbers = [1, 2, 3, 4];
if (numbers is [1, 2, <5, >=4])
    Console.WriteLine("List pattern: matches 1, 2 at the start");

if (numbers is [1, 2, .. var rest])
    Console.WriteLine($"List pattern: Starts with 1, 2. Rest: {string.Join(",", rest)}");

// 10. List Pattern with Spans and Ranges (since C# 12.0)
Span<int> spanNumbers = stackalloc[] { 10, 20, 30, 40, 50 };
if (spanNumbers is [10, .. var middle, 50])
    Console.WriteLine($"List pattern with Span: Starts with 10, ends with 50. Middle: {string.Join(",", [.. middle ])}");

// Helper types and methods

static IEnumerable<Person> GetPeople() =>
    [
        new Person("Clark", "Kent", Address: new("Smallville", "USA")),
        new Person("Lois", "Lane", Address: new("Smallville", "USA")),
        new Person("Bruce", "Wayne", Address: new("Gotham City", "USA")),
        new Person("Alfred", "Pennyworth", Address: new("Gotham City", "USA")),
        new Person("Dick", "Grayson", Address: new("Gotham City", "USA")),
        new Person("Barry", "Allen", Address: new("Central City", "USA")),
    ];

public record class Address(string City, string Country);

public record class Person(string FirstName, string LastName, Address? Address = default)
{
    public override string ToString() => $"{FirstName} {LastName}";
}

public readonly record struct Point(int X, int Y);
