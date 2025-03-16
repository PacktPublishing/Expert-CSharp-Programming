int[] numbers = [1, 2, 3, 4, 5];
List<string> words = ["one", "two", "three", "four"];
IEnumerable<Person> people =[new Person("John", "Doe"), new Person("Jane", "Doe")];

List<int> numbers2 = [1, 2, 3, 4, 5];
List<int> combined = [.. numbers, .. numbers2];

public record class Person(string FirstName, string LastName);
