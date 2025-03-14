﻿using System.Diagnostics.CodeAnalysis;

public ref struct SpanIterator<T>(Span<T> span)
{
    private readonly Span<T> _span = span;
    private int _index = -1;

    public bool MoveNext([MaybeNullWhen(false)] out T value)
    {
        _index++;
        if (_index >= 0 && _index < _span.Length)
        {
            value = _span[_index];
            return true;
        }
        value = default;
        return false;
    }

    public ref T Current
    {
        get
        {
            if (_index < 0 || _index >= _span.Length)
            {
                throw new InvalidOperationException();
            }
            return ref _span[_index];
        }
    }
}
