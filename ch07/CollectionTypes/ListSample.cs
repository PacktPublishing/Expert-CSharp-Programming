using System.Runtime.InteropServices;

namespace CollectionTypes;

public class ListSample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(ListSample));

        // ShowCapacity();
        InsertItems();
    }

    private static void ShowCapacity()
    {
        List<SomeValue> list = [];
        // list.EnsureCapacity(5000);
        int currentCapacity = list.Capacity;
        Console.WriteLine($"List capacity: {currentCapacity}");

        for (int i = 1; i < 8000; i++)
        {
            list.Add(new SomeValue(i));
            if (list.Capacity != currentCapacity)
            {
                Console.WriteLine($"List capacity changed from {currentCapacity} to {list.Capacity} adding item {i}");
                currentCapacity = list.Capacity;
            }
        }

        Console.WriteLine();
    }

    private static void InsertItems()
    {
        List<int> list = [1, 2, 3, 4, 5];
        list.EnsureCapacity(50);

        ShowListElementsAddresses("before insert", list);
        list.Insert(3, 42);
        ShowListElementsAddresses("after insert", list);
    }

    private unsafe static void ShowListElementsAddresses<T>(
        string message, List<T> list)
    {
        Console.WriteLine(message);

        // Get a reference to the underlying collection
        var span = CollectionsMarshal.AsSpan(list);

        for (int i = 0; i < span.Length; i++)
        {
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
            fixed (T* p = &span[i])
            {
                Console.WriteLine(
                  $"Element {i} in the list: {(nint)p:X} with value {span[i]}");
            }
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
        }
        Console.WriteLine();
    }
}
