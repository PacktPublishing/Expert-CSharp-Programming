using Microsoft.Extensions.Logging;

namespace WeakEvents;

public class Observer(ILogger<Observer> logger)
{
    public void Handler(object? sender, SubjectEventArgs args)
    {
        logger.LogInformation("Received event from {Id}", args.Id);
    }
}
