using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Microsoft.Extensions.Logging.Abstractions;

using System.Windows;

using WeakEvents;

BenchmarkRunner.Run<EventsBenchmark>();
//EventsBenchmark eventsBenchmark = new();
//eventsBenchmark.StrongEvents();
// eventsBenchmark.WeakEventsWeakEvent();

[MemoryDiagnoser]
public class EventsBenchmark
{
    [Benchmark]
    public void StrongEvents()
    {
        const int observerCount = 100;
        Observer?[]? observers = Enumerable.Range(1, observerCount).Select(i => new Observer(NullLogger<Observer>.Instance)).ToArray();
        //        var observer = new Observer(NullLogger<Observer>.Instance);
        Subject subject = new(1);
        foreach (var observer in observers)
        {
            subject.SomeEvent += observer!.Handler;
        }
        subject.RaiseEvent();
        //        subject.SomeEvent += observer.Handler;
        //        subject.RaiseEvent();
        for (int i = 0; i < observerCount; i++)
        {
            observers[i] = null;
        }
        observers = null;
        //        observer = null;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: false);
        GC.WaitForFullGCComplete();
        subject.RaiseEvent();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: false);
        GC.WaitForFullGCComplete();
        subject.RaiseEvent();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: false);
        GC.WaitForFullGCComplete();

    }

    [Benchmark]
    public void WeakEventsWeakEvent()
    {
        const int observerCount = 100;
        Observer?[]? observers = Enumerable.Range(1, observerCount).Select(i => new Observer(NullLogger<Observer>.Instance)).ToArray();
        //        var observer = new Observer(NullLogger<Observer>.Instance);
        Subject2 subject = new(1);
        foreach (var observer in observers)
        {
            subject.SomeEvent += observer!.Handler;
        }
        subject.RaiseEvent();
        for (int i = 0; i < observerCount; i++)
        {
            observers[i] = null;
        }
        observers = null;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: false);
        GC.WaitForFullGCComplete();
        subject.RaiseEvent();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: false);
        GC.WaitForFullGCComplete();
        subject.RaiseEvent();
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: false);
        GC.WaitForFullGCComplete();
    }
}