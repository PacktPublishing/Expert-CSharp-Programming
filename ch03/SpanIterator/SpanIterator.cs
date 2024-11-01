using System.Diagnostics.CodeAnalysis;

public ref struct SpanIterator<T>(Span<T> span)
{
    private readonly Span<T> _span = span;
    private int _index = 0;

    public bool MoveNext([MaybeNullWhen(false)] out T value)
    {
        if (_index < _span.Length)
        {
            value = _span[_index++];
            return true;
        }
        value = default;
        return false;
    }
}
