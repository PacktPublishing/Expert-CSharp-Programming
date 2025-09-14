using System.Runtime.CompilerServices;

namespace Traits;

[CollectionBuilder(typeof(CustomCollectionBuilder), "Create")]
public interface IEnumerableEx<T> : IEnumerable<T>
{
    /// <summary>
    /// Filters elements based on a predicate. Implementation may vary between lazy and eager evaluation.
    /// </summary>
    /// <param name="predicate">The condition to test each element</param>
    /// <returns>Filtered elements</returns>
    IEnumerable<T> Where(Func<T, bool> predicate);
}

/// <summary>
/// Extension methods providing default implementations for IEnumerableEx
/// </summary>
public static class EnumerableExExtensions
{
    /// <summary>
    /// Default lazy implementation of Where for IEnumerableEx
    /// </summary>
    public static IEnumerable<T> DefaultWhere<T>(this IEnumerableEx<T> source, Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}
