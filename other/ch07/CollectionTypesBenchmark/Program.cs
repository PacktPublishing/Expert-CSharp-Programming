using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkCollectionTypes>();

//[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkCollectionTypes
{
    private List<Data>? _list;
    private Queue<Data>? _queue;
    private LinkedList<Data>? _linkedList;
    private LinkedListNode<Data>? _midItem;

    private List<Value>? _valueList;
    private LinkedList<Value>? _valueLinkedList;
    private int _midNumber;

    [Params(10000, 100000, 1000000)]
    public int N;

    [IterationSetup]
    public void Setup()
    {
        _midNumber = N / 2;
        _list = [];
        _queue = new();
        _linkedList = new();
        _valueList = [];
        _valueLinkedList = new();

        for (int i = 0; i < N; i++)
        {
            _list.Add(new Data(i, i + 1, i + 2));
            _valueList.Add(new Value(i, i + 1, i + 2));
            _queue.Enqueue(new Data(i, i + 1, i + 2));
            _linkedList.AddLast(new Data(i, i + 1, i + 2));
            _valueLinkedList.AddLast(new Value(i, i + 1, i + 2));
            if (i == _midNumber)
            {
                _midItem = _linkedList.Last;
            }
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

    //[Benchmark]
    //public void InsertInList()
    //{
    //     _list!.Insert(_midNumber, new Data(900, 901, 902));
    //}

    [Benchmark]
    public void ReverseList()
    {
        _list!.Reverse();
    }

    [Benchmark]
    public void ReverseLinkedList()
    {
        _linkedList!.Reverse();
    }

    [Benchmark]
    public void ReverseValueList()
    {
        _valueList!.Reverse();
    }

    [Benchmark]
    public void ReverseValueLinkedList()
    {
        _valueLinkedList!.Reverse();
    }

    //[Benchmark]
    //public void InsertInLinkedList()
    //{
    //    _linkedList!.AddAfter(_midItem!, new Data(900, 901));
    //}

    //[Benchmark]
    //public void CountList()
    //{
    //    var count = _list!.Count;
    //}

    //[Benchmark]
    //public void CountQueue()
    //{
    //    var count = _queue!.Count;
    //}

    //[Benchmark]
    //public void CountLinkedList()
    //{
    //    var count = _linkedList!.Count;
    //}


    //[Benchmark]
    //public void CountListMethod()
    //{
    //    var count = _list!.Count();
    //}

    //[Benchmark]
    //public void CountQueueMethod()
    //{
    //    var count = _queue!.Count();
    //}

    //[Benchmark]
    //public void CountLinkedListMethod()
    //{
    //    var count = _linkedList!.Count();
    //}
}

public record struct Value(long X, long Y, long Z)
{
    public decimal A { get; set; }
    public decimal B { get; set; }
    public decimal C { get; set; }
}

public record class Data(long X, long Y, long Z)
{
    public decimal A { get; set; }
    public decimal B { get; set; }
    public decimal C { get; set; }
}