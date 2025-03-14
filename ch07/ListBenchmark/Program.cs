using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkLists>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkLists
{
    private List<int>? _list;
    private List<int>? _listSized;
    private int[]? _array;

    [Params(100, 1000, 10000, 100000)]
    public int N;

    [IterationSetup]
    public void Setup()
    {
        _list = new List<int>();
        _array = new int[N];
        _listSized = new List<int>(capacity: N);
    }

    [Benchmark]
    public void ListAdd()
    {
        int i = 0;
        try
        {
            for (; i < N; i++)
            {
                _list!.Add(i);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(i);
        }
    }

    [Benchmark]
    public void ListSizedAdd()
    {
        for (int i = 0; i < N; i++)
        {
            _listSized!.Add(i);
        }
    }

    [Benchmark]
    public void ArrayAdd()
    {
        for (int i = 0; i < N; i++)
        {
            _array![i] = i;
        }
    }
}