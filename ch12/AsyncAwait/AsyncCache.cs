namespace AsyncAwait;

/// <summary>
/// Simple in-memory cache that returns <see cref="ValueTask{T}"/> to avoid
/// a heap allocation on the hot (cache-hit) path.
/// </summary>
/// <remarks>
/// Not thread-safe — intended for sequential use only (as shown in the ValueTask demo
/// where the same key is fetched twice sequentially to illustrate the sync vs async path).
/// For concurrent access, replace <see cref="Dictionary{TKey,TValue}"/> with
/// <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/> and
/// use a <c>lock</c> or <see cref="SemaphoreSlim"/> around the async fetch to avoid
/// duplicate in-flight fetches for the same key.
/// </remarks>
internal sealed class AsyncCache<T>
{
    private readonly Dictionary<string, T> _store = [];

    public ValueTask<T> GetOrFetchAsync(string key, Func<Task<T>> factory)
    {
        if (_store.TryGetValue(key, out T? cached))
            return new ValueTask<T>(cached); // synchronous — no allocation

        return new ValueTask<T>(FetchAndStoreAsync(key, factory));
    }

    private async Task<T> FetchAndStoreAsync(string key, Func<Task<T>> factory)
    {
        T value = await factory();
        _store[key] = value;
        return value;
    }
}
