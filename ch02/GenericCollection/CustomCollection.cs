using System.Collections;

namespace GenericCollection;
internal class CustomCollection<T> : IReadOnlyList<T>
{
    private readonly T[] _items;

    public CustomCollection(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(capacity, Array.MaxLength);
        if (capacity == 0)
        {
            _items = [];
            return;
        }
        _items = new T[capacity];
    }

    public T this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }

    public int Count =>
        _items.Count(x => x != null);

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items[i] is not null)
            {
                yield return _items[i];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        _items.GetEnumerator();
}
