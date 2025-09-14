namespace Traits;

public class CachedList<T> : List<T>, IEnumerableEx<T>
{
    private readonly Dictionary<string, List<T>> _cache = [];
    private readonly object _cacheLock = new();

    public new void Add(T item)
    {
        base.Add(item);
        InvalidateCache();
    }

    public new void AddRange(IEnumerable<T> collection)
    {
        base.AddRange(collection);
        InvalidateCache();
    }

    public new bool Remove(T item)
    {
        var result = base.Remove(item);
        if (result)
        {
            InvalidateCache();
        }
        return result;
    }

    public new void Clear()
    {
        base.Clear();
        InvalidateCache();
    }

    private void InvalidateCache()
    {
        lock (_cacheLock)
        {
            _cache.Clear();
        }
    }

    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        // Create a simple string representation of the predicate for caching
        // Note: This is still not perfect but demonstrates the concept
        var predicateKey = predicate.Method.ToString();
        
        lock (_cacheLock)
        {
            if (_cache.TryGetValue(predicateKey, out var cachedResult))
            {
                return cachedResult;
            }

            List<T> result = [];
            foreach (var item in this)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            
            _cache.Add(predicateKey, result);
            return result;
        }
    }
}
