using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Traits;

[CollectionBuilder(typeof(CustomCollectionBuilder), "Create")]
public class CustomCollection<T> : Collection<T>, IEnumerableEx<T>
{
}

public class CustomCollectionBuilder
{
    public static CustomCollection<T> Create<T>(params ReadOnlySpan<T> items)
    {
        // don't refactor for IDE0028 - collection initialization creates an recursive loop with a collection builder
        // https://github.com/dotnet/roslyn/issues/70099
#pragma warning disable IDE0028 // Simplify collection initialization
        CustomCollection<T> collection = new();
        foreach (T item in items)
        {
            collection.Add(item);
        }
#pragma warning restore IDE0028 // Simplify collection initialization
        return collection;
    }
}
