namespace WeakEvents;

public class WeakEvent<T>
    where T : EventArgs
{
    private readonly List<WeakReference<EventHandler<T>>> _handlers = [];

    public void AddHandler(EventHandler<T> handler)
    {
        _handlers.Add(new WeakReference<EventHandler<T>>(handler));
    }
    public void RemoveHandler(EventHandler<T> handler)
    {
        _handlers.RemoveAll(wr =>
        {
            if (wr.TryGetTarget(out var target))
            {
                return target == handler;
            }
            return false;
        });
    }

    public void RaiseEvent(object sender, T args)
    {
        foreach (var weakReference in _handlers.ToList())
        {
            if (weakReference.TryGetTarget(out var handler))
            {
                handler(sender, args);
            }
            else
            {
                _handlers.Remove(weakReference);
            }
        }
    }
}
