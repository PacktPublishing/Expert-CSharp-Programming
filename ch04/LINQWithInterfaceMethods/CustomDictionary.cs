namespace LINQWithInterfaceMethods;

public class CustomDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IEnumerableEx<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>> Where(Func<KeyValuePair<TKey, TValue>, bool> predicate)
    {
        foreach (var item in this)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}
