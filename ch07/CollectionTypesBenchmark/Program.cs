using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkCollectionTypes>();

//[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkCollectionTypes
{
    private List<int>? _list;
    private Queue<int>? _queue;
    private LinkedList<int>? _linkedList;
    private int _middle;

    [Params(1000, 10000)]
    public int N;

    [IterationSetup]
    public void Setup()
    {
        _middle = N / 2;
        _list = [];
        _queue = new Queue<int>();
        _linkedList = new LinkedList<int>();
        for (int i = 0; i < N; i++)
        {
            _list.Add(i);
            _queue.Enqueue(i);
            _linkedList.AddLast(i);
        }
    }

    //[Benchmark]
    //public void QueueRemoveItem()
    //{
    //    for (int i = 0; i < 100; i++)
    //    {
    //        _queue!.Dequeue();
    //    }
    //}

    //[Benchmark]
    //public void ListRemoveItem()
    //{
    //    int max = _middle + 100;
    //    for (int i = _middle; i < max; i++)
    //    {
    //        _list!.Remove(i);
    //    }
    //}

    //[Benchmark]
    //public void LinkedListRemoveItem()
    //{
    //    int max = _middle + 100;
    //    for (int i = _middle; i < max; i++)
    //    {
    //        _linkedList!.Remove(i);
    //    }
    //}

    [Benchmark]
    public void CountList()
    {
        var count = _list!.Count;
    }

    [Benchmark]
    public void CountQueue()
    {
        var count = _queue!.Count;
    }

    [Benchmark]
    public void CountLinkedList()
    {
        var count = _linkedList!.Count;
    }


    [Benchmark]
    public void CountListMethod()
    {
        var count = _list!.Count();
    }

    [Benchmark]
    public void CountQueueMethod()
    {
        var count = _queue!.Count();
    }

    [Benchmark]
    public void CountLinkedListMethod()
    {
        var count = _linkedList!.Count();
    }
}