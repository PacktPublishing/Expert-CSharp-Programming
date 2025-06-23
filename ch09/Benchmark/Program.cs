using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<TestPatterns>();

//[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class TestPatterns
{
    [Params(2, 300, 301, 302, 303, 304, 399)]
    public short Value { get; set; }

    [Benchmark(Baseline = true)]
    public bool NotAPattern() => Sample1(Value);

    [Benchmark] 
    public bool IsOrPattern() => Sample2(Value);

    [Benchmark] 
    public bool SwitchExpression() => Sample3(Value);

    private static bool Sample1(short version)
        => version == 0x0002
        || version == 0x0300
        || version == 0x0301
        || version == 0x0302
        || version == 0x0303
        || version == 0x0304;

    private static bool Sample2(short version)
        => version is 0x0002
                   or 0x0300
                   or 0x0301
                   or 0x0302
                   or 0x0303
                   or 0x0304;

    private static bool Sample3(short version)
        => version switch
        {
            0x0002 or 0x0300 or 0x0301 or 0x0302 or 0x0303 or 0x0304 => true,
            _ => false
        };
}