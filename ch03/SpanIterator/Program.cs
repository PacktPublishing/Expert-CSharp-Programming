int[] data = [1, 5, 8, 11, 22, 23, 27, 33];

Span<int> span = data.AsSpan();

SpanIterator<int> iterator = new SpanIterator<int>(span);
while (iterator.MoveNext(out int value))
{
    Console.WriteLine(value);
}
