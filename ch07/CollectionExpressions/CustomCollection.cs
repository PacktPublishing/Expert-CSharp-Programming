using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace CollectionExpressions;

[CollectionBuilder(typeof(CustomCollection), nameof (CustomCollection.Create))]
public class CustomCollection<T> : Collection<T>
{
    public T[] this[Range range]
    {
        get
        {        
            var (offset, length) = range.GetOffsetAndLength(Count);
            return [.. Items.Skip(offset).Take(length)];
        }
    }
}

public static class CustomCollection
{
    public static CustomCollection<T> Create<T>(params ReadOnlySpan<T> items)
    {
// #pragma warning disable IDE0028 // Simplify collection initialization
        // applying this analyzer recommendation here results in recursive invocations: https://github.com/dotnet/roslyn/issues/70099
        CustomCollection<T> collection = new();
// #pragma warning restore IDE0028 // Simplify collection initialization
        foreach (var item in items)
        {
            collection.Add(item);
        }
        return collection;
    }
}
