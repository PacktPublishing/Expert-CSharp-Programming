int[] data = [1, 5, 8, 11, 22, 23, 27, 33];

Span<int> span = data; // Implicit conversion from int[] to Span<int>

SpanIterator<int> iterator = new(span);
while (iterator.MoveNext(out int value))
{
    Console.Write($"{value} ");
}
Console.WriteLine();

iterator = new(span);
while (iterator.MoveNext(out _))
{
    ref int item = ref iterator.Current;
    if (item % 2 == 0)
    {
        item *= 2;
    }
}

Console.WriteLine("Modified the array");
foreach (int item in data)
{
    Console.Write($"{item} ");
}
Console.WriteLine();
