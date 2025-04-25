namespace CollectionTypes;

public class LinkedListSample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(LinkedListSample));

        LinkedList<int> linkedList = new();
        linkedList.AddLast(1);
        var second = linkedList.AddLast(2);
        linkedList.AddLast(3);
        ShowLinkedListElementsAddresses("before insert", linkedList);

        linkedList.AddAfter(second, 4);
            
        ShowLinkedListElementsAddresses("after insert", linkedList);

        Console.WriteLine();
    }

    private unsafe static void ShowLinkedListElementsAddresses<T>(
        string message, LinkedList<T> linkedList)
        where T : struct
    {
        Console.WriteLine(message);

        LinkedListNode<T>? current = linkedList.First;
        int index = 0;

        while (current is not null)
        {
            ref T node = ref current.ValueRef;
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
            fixed (T* p = &node)
            {
                Console.WriteLine(
                    $"Element {index} with value {current.Value} at the address {(nint)p:X}"); ;
            }
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

            current = current.Next;
            index++;
        }

        Console.WriteLine();
    }
}
