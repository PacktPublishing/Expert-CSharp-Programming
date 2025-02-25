using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Microsoft.Extensions.Logging.Abstractions;

using System.Windows;

using WeakEvents;

BenchmarkRunner.Run<EventsBenchmark>();

[MemoryDiagnoser]
public class EventsBenchmark
{
    [Benchmark]
    public void StrongEvents()
    {
        Subject? subject = new(1);
        var observer = new Observer(NullLogger<Observer>.Instance);
        subject.SomeEvent += observer.Handler;
        subject.RaiseEvent();
        subject = null;
    }

    [Benchmark]
    public void WeakEvents()
    {
        Subject? subject = new(1);
        var observer = new Observer(NullLogger<Observer>.Instance);
        WeakEventManager<Subject, SubjectEventArgs>.AddHandler(subject, "SomeEvent", observer.Handler);
        subject.RaiseEvent();
        subject = null;
    }
}