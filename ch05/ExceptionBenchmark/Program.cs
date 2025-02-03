using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<BenchmarkExceptions>();

[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkExceptions
{
    [Benchmark]
    public void ThrowAndCatch()
    {
        ParseWithException();
    }

    [Benchmark]
    public void StatusCode()
    {
        ParseWithStatusCode();
    }

    internal static void ParseWithException()
    {
        string test = "test";
        int i = 0;
        try
        {
            int result = int.Parse(test);
        }
        catch (FormatException)
        {
            i++;
        }
        catch (OverflowException)
        {
            i++;
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal static void ParseWithStatusCode()
    {
        int i = 0;
        string test = "test";
        if (int.TryParse(test, out int result))
        {
            i++;
        }
    }
}
