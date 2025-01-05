using System.Runtime.CompilerServices;

namespace LINQWithInterfaceMethods;

[CollectionBuilder(typeof(CustomCollectionBuilder), "Create")]
public interface IEnumerableEx<T> : IEnumerable<T>
{
    public virtual IEnumerable<T> Where(Func<T, bool> predicate)
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
