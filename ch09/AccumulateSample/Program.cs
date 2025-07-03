using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

using System.Numerics;

//int[] numbers = [1, 3, 5, 11, 22, 33, 42, 44, 46];

//int result = Accumulate(numbers);
//Console.WriteLine(result);

//result = AccumulateGeneric(numbers);
//Console.WriteLine($"Using generic {result}");

//result = AccumulateRecursive(numbers);
//Console.WriteLine($"Using recursive {result}");

//result = AccumulateRecursiveSpan(numbers);  // numbers.AsSpan with .NET 9, native type support for Span with.NET 10
//Console.WriteLine($"Using recursive with Span {result}");

//static int AccumulateFor(int[] values)
//{
//    int sum = 0;
//    for (int i = 0; i < values.Length; i++)
//    {
//        sum += values[i];
//    }
//    return sum;
//}

//static int AccumulateForeach(int[] values)
//{
//    int sum = 0;
//    foreach (int n in values)
//        sum += n;
//    return sum;
//}

//static T AccumulateGeneric<T>(T[] values)
//    where T : INumber<T>
//{
//    T sum = T.Zero;
//    foreach (T n in values)
//    {
//        sum += n;
//    }
//    return sum;
//}

//static T AccumulateRecursive<T>(T[] values)
//    where T : INumber<T> =>
//    values switch
//    {
//        [] => T.Zero,
//        [var first, .. var rest] => first + AccumulateRecursive(rest),
//    };


//static T AccumulateRecursiveSpan<T>(Span<T> values)
//    where T : INumber<T> =>
//    values switch
//    {
//        [] => T.Zero,
//        [var first, .. var rest] => first + AccumulateRecursiveSpan(rest),
//    };

// Benchmarking code

var summary = BenchmarkRunner.Run<BenchmarkAccumulate>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
[MemoryDiagnoser]
public class BenchmarkAccumulate
{
    private readonly int[] _data = [.. Enumerable.Range(1, 100)];

    [Benchmark(Baseline = true)]
    public int AccumulateFor() => AccumulateFor(_data);

    [Benchmark()]
    public int AccumulateForeach() => Accumulate(_data);

    [Benchmark()]
    public int AccumulateGeneric() => AccumulateGeneric(_data);

    [Benchmark()]
    public int AccumulateRecursive() => AccumulateRecursive(_data);

    [Benchmark()]
    public int AccumulateRecursiveSpan() => AccumulateRecursiveSpan(_data.AsSpan());

    static int AccumulateFor(int[] values)
    {
        int sum = 0;
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }
        return sum;
    }

    static int Accumulate(int[] values)
    {
        int sum = 0;
        foreach (int n in values)
            sum += n;
        return sum;
    }

    static T AccumulateGeneric<T>(T[] values)
        where T : INumberBase<T>
    {
        T sum = T.Zero;
        foreach (T n in values)
        {
            sum += n;
        }
        return sum;
    }

    static T AccumulateRecursive<T>(T[] values)
        where T : INumberBase<T> =>
        values switch
        {
            [] => T.Zero,
            [var first, .. var rest] => first + AccumulateRecursive(rest),
        };

    static T AccumulateRecursiveSpan<T>(Span<T> values)
        where T : INumberBase<T> =>
        values switch
        {
            [] => T.Zero,
            [var first, .. var rest] => first + AccumulateRecursiveSpan(rest),
        };
}