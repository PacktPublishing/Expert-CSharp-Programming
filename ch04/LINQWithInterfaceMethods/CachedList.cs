namespace LINQWithInterfaceMethods;

public class CachedList<T> : List<T>, IEnumerableEx<T>
{
    private readonly Dictionary<Func<T, bool>, IEnumerableEx<T>> _cache = [];

    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        if (_cache.TryGetValue(predicate, out var cachedResult))
        {
            return cachedResult;
        }

        CachedList<T> result = [];
        foreach (var item in this)
        {
            if (predicate(item))
            {
                result.Add(item);
            }
        }
        _cache.Add(predicate, result);
        return result;
    }
}
