using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

// Create a fast benchmark configuration to reduce execution time
var config = ManualConfig
    .Create(DefaultConfig.Instance)
    .WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend))
    .AddExporter(MarkdownExporter.GitHub)
    .AddColumn(new AverageAcrossParametersColumn())
    .AddExporter(new ParameterAverageExporter());

Console.WriteLine("Running optimized benchmarks...");
var summary = BenchmarkRunner.Run<TestPatterns>(config);

/// <summary>
/// Custom column that calculates the average across all parameter values for each method/runtime combination
/// </summary>
public class AverageAcrossParametersColumn : IColumn
{
    public string Id => nameof(AverageAcrossParametersColumn);
    public string ColumnName => "Avg across params";
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Statistics;
    public int PriorityInCategory => 100;
    public bool IsNumeric => true;
    public UnitType UnitType => UnitType.Time;
    public string Legend => "Average value across all parameter values";
    
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var methodName = benchmarkCase.Descriptor.WorkloadMethod.Name;
        var runtime = benchmarkCase.Job.Environment.Runtime?.Name ?? null;
        
        // Get all benchmarks with the same method and runtime but different parameter values
        var relatedBenchmarks = summary.Reports
            .Where(r => r.BenchmarkCase.Descriptor.WorkloadMethod.Name == methodName &&
                        r.BenchmarkCase.Job.Environment.Runtime?.Name == runtime)
            .ToList();
        
        if (relatedBenchmarks.Count == 0)
            return "-";
        
        // Calculate the average mean value across all parameter values
        var avgMean = relatedBenchmarks
            .Where(r => r.ResultStatistics != null)
            .Select(r => r.ResultStatistics?.Mean ?? 0)
            .DefaultIfEmpty(0)
            .Average();
        
        return avgMean.ToString("N3") + " ns";
    }

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) 
        => GetValue(summary, benchmarkCase);

    public bool IsAvailable(Summary summary) => true;
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;
}

/// <summary>
/// Custom exporter that displays averages across parameter values in a separate table
/// </summary>
public class ParameterAverageExporter : IExporter
{
    public string Name => nameof(ParameterAverageExporter);

    public void ExportToLog(Summary summary, ILogger logger)
    {
        if (!summary.Reports.Any())
            return;

        // Group benchmark results by method name and runtime
        var groupedResults = summary.Reports
            .GroupBy(r => new 
            { 
                Method = r.BenchmarkCase.Descriptor.WorkloadMethod.Name,
                Runtime = r.BenchmarkCase.Job.Environment.Runtime?.Name
            })
            .Select(g => new
            {
                g.Key.Method,
                g.Key.Runtime,
                AverageMean = g.Where(r => r.ResultStatistics != null)
                               .Select(r => r.ResultStatistics?.Mean ?? 0)
                               .DefaultIfEmpty(0)
                               .Average()
            })
            .OrderBy(x => x.Runtime)
            .ThenBy(x => x.Method);

        logger.WriteLine();
        logger.WriteLine("=== Average Results Across All Parameter Values ===");
        logger.WriteLine();
        logger.WriteLine("| Method | Runtime | Average (ns) |");
        logger.WriteLine("|--------|---------|-------------|");
        
        foreach (var result in groupedResults)
        {
            logger.WriteLine($"| {result.Method} | {result.Runtime} | {result.AverageMean:N3} |");
        }
        
        logger.WriteLine();
    }
    
    // Implementing the required ExportToFiles method from IExporter interface
    public IEnumerable<string> ExportToFiles(Summary summary, ILogger consoleLogger)
    {
        var filePath = Path.Combine(summary.ResultsDirectoryPath, "averages-summary.md");
        using (var streamWriter = new StreamWriter(filePath))
        {
            var fileLogger = new StreamLogger(streamWriter);
            ExportToLog(summary, fileLogger);
        }
        
        return [filePath];
    }
}

// Use SimpleJob with shortened run configuration
[SimpleJob(RuntimeMoniker.Net80, launchCount: 1, warmupCount: 1, iterationCount: 3, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 1, iterationCount: 3)]
[SimpleJob(RuntimeMoniker.Net10_0, launchCount: 1, warmupCount: 1, iterationCount: 3)]
// Use InProcess toolchain for faster benchmarks
[InProcessAttribute]
public class TestPatterns
{
    // Reduce parameter set for faster execution by selecting key values
    [Params(0x2, 0x300, 0x399)] // Reduced from 7 to 3 values
    public short Value { get; set; }

    [Benchmark(Baseline = true)]
    public bool NotAPattern() => NotAPattern(Value);

    [Benchmark] 
    public bool IsOrPattern() => IsOr(Value);

    [Benchmark] 
    public bool SwitchExpression() => SwitchExpression(Value);

    private static bool NotAPattern(short version)
        => version == 0x0002
        || version == 0x0300
        || version == 0x0301
        || version == 0x0302
        || version == 0x0303
        || version == 0x0304;

    private static bool IsOr(short version)
        => version is 0x0002
                   or 0x0300
                   or 0x0301
                   or 0x0302
                   or 0x0303
                   or 0x0304;

    private static bool SwitchExpression(short version)
        => version switch
        {
            0x0002 or 0x0300 or 0x0301 or 0x0302 or 0x0303 or 0x0304 => true,
            _ => false
        };
}