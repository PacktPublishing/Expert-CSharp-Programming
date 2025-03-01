using Microsoft.Extensions.Logging;

namespace WeakEvents;

public class Observer
{
    private readonly ILogger<Observer> logger;
    private string _data = "observer 1";
    private byte[] _data2 = new byte[4096];
    private byte[] _data3 = new byte[1024];
    private byte[] _data4 = new byte[4096];
    private byte[] _data5 = new byte[1024];

    public Observer(ILogger<Observer> logger)
    {
        this.logger = logger;
        GC.AddMemoryPressure(1024 * 1024);
    }

    public void Handler(object? sender, SubjectEventArgs args)
    {
        _data2.AsSpan().Fill(42);
        _data3.AsSpan().Fill(42);
        logger.LogInformation("{data} received event from {Id}", _data, args.Id);
    }
}
