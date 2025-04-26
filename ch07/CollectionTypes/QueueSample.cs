
using System.Runtime.InteropServices;

namespace CollectionTypes;

public class QueueSample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(QueueSample));

        Queue<SomeValue> queue = new();
        Console.WriteLine(queue.Capacity);
        queue.Enqueue(new SomeValue(1));
        queue.Enqueue(new SomeValue(2));
        queue.Enqueue(new SomeValue(3));
        Utilities.ShowMemoryAddress(ref queue);
        Console.WriteLine($"First: {queue.Peek()}");
        Console.WriteLine($"Dequeue: {queue.Dequeue()}");
        Console.WriteLine($"Dequeue: {queue.Dequeue()}");
        Console.WriteLine($"Dequeue: {queue.Dequeue()}");
        Console.WriteLine();
    }
}

