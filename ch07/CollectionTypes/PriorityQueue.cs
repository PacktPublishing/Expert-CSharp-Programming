namespace CollectionTypes;

public class PriorityQueueSample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(PriorityQueueSample));
        PriorityQueue<SomeData, long> queue = new();
        for (int i = 1; i < 100; i++)
        {
            queue.Enqueue(new SomeData(i), i % 3);
        }

//        Utilities.ShowMemoryAddress(ref queue);
        Console.WriteLine($"First: {queue.Peek()}");
        while (queue.Count > 0)
        {
            SomeData data = queue.Dequeue();
            Console.WriteLine($"Dequeue: {queue.Dequeue()}");
        }
        Console.WriteLine();
    }
}
