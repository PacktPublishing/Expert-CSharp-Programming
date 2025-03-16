using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkListElementSize>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkListElementSize
{
    private SomeClassData[]? _classArray;
    private SomeStructData[]? _structArray;

    private List<SomeClassData>? _classList;
    private List<SomeStructData>? _structList;

    [Params(100, 1000, 10000, 100000, 1000000, 10000000)]
    public int N;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _classArray = Enumerable.Range(0, N).Select(i => new SomeClassData(i, i, i)).ToArray();
        _structArray = Enumerable.Range(0, N).Select(i => new SomeStructData(i, i, i)).ToArray();
    }

    [IterationSetup]
    public void Setup()
    {
        _classList = [];
        _structList = [];
    }

    [Benchmark]
    public void ClassAdd()
    {
        for (int i = 0; i < N; i++)
        {
            _classList!.Add(_classArray![i]);
        }
    }

    [Benchmark]
    public void StructAdd()
    {
        for (int i = 0; i < N; i++)
        {
            _structList!.Add(_structArray![i]);
        }
    }
}

public class SomeClassData(int x, int y, int z)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
    public long Z { get; set; } = z;
    public long X1 { get; set; } = x;
    public long Y1 { get; set; } = y;
    public long Z1 { get; set; } = z;
}

public struct SomeStructData(int x, int y, int z)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
    public long Z { get; set; } = z;
    public long X1 { get; set; } = x;
    public long Y1 { get; set; } = y;
    public long Z1 { get; set; } = z;
}