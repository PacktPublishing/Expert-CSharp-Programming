using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


BenchmarkRunner.Run<BenchmarkLogging>();

[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class BenchmarkLogging
{
    private Runner? _runner;
    private readonly string[] _text = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"];

    [GlobalSetup]
    public void GlobalSetup()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddTransient<Runner>();
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddProvider(NullLoggerProvider.Instance);
            loggingBuilder.SetMinimumLevel(LogLevel.Information);
        });
        var app = builder.Build();
        _runner = app.Services.GetRequiredService<Runner>();

        _runner.HighPerformanceLogging(Guid.NewGuid(), "test");
    }

    [Benchmark]
    public void DoNotUseStringInterpolationLogging()
    {
        if (_runner is null) throw new InvalidOperationException();
        for (int i = 0; i < 100; i++)
        {
            _runner.DoNotUseStringInterpolationLogging(Guid.NewGuid(), _text[i % 10]);
        }
    }

    [Benchmark]
    public void StructuredLogging()
    {
        if (_runner is null) throw new InvalidOperationException();
        for (int i = 0; i < 100; i++)
        {
            _runner.StructuredLogging(Guid.NewGuid(), _text[i % 10]);
        }

    }

    [Benchmark]
    public void HighPerformanceLogging()
    {
        if (_runner is null) throw new InvalidOperationException();
        for (int i = 0; i < 100; i++)
        {
            _runner.HighPerformanceLogging(Guid.NewGuid(), _text[i % 10]);
        }
    }   
}

internal class Runner(ILogger<Runner> logger)
{
    public void DoNotUseStringInterpolationLogging(Guid id, string text)
    {
        // don't use string interpolation with logging
        logger.LogInformation($"Log message with {id} and {text}");
    }

    public void StructuredLogging(Guid id, string text)
    {
        logger.LogInformation("Log message with {Id} and {Text}", id, text);
    }

    public void HighPerformanceLogging(Guid id, string text)
    {
        logger.LogHighPerformanceLogging(id, text);
    }

}

internal static partial class LoggingExtension
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Log message with {Id} and {Text}")]
    public static partial void LogHighPerformanceLogging(this ILogger logger, Guid id, string text);

    [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Log message with {Id}")]
    public static partial void LogHighPerformanceLogging2(this ILogger logger, Guid id);
}