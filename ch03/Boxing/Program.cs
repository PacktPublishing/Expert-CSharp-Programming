
using System.Runtime.CompilerServices;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkList>();

[MemoryDiagnoser]
public class BenchmarkList
{
    [Benchmark]
    public void Boxing()
    {
        BoxingList();
    }

    [Benchmark]
    public void Values()
    {
        ValuesList();
    }

    private static int BoxingList()
    {
        List<object> list = new(capacity: 1000);
        for (int i = 0; i < 1000; i++)
        {
            list.Add(i);
        }

        int sum = 0;
        foreach (int item in list)
        {
            sum += item;
        }
        return sum;
    }

    private static int ValuesList()
    {
        List<int> list = new(capacity: 1000);
        for (int i = 0; i < 1000; i++)
        {
            list.Add(i);
        }

        int sum = 0;
        foreach (int item in list)
        {
            sum += item;
        }
        return sum;
    }
}
