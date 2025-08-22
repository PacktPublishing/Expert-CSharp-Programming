int[] data = [1, 5, 8, 11, 22, 23, 27, 33];

ReadOnlySpan<int> span = data;

ReadOnlySpanIterator<int> iterator = new(span);
while (iterator.MoveNext(out int value))
{
    Console.Write($"{value} ");
}

iterator = new(span);
while (iterator.MoveNext(out _))
{
    ref readonly int item = ref iterator.Current;
    Console.Write($"{item} ");
}
