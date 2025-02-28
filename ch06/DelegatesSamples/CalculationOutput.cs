namespace Delegates;

internal class CalculationOutput(TextWriter writer)
{
    public void Add(int x, int y)
    {
        writer.WriteLine($"{x} + {y} results in {x + y}");
    }

    public void Subtract(int x, int y)
    {
        writer.WriteLine($"{x} - {y} results in {x - y}");
    }

    public static void ThrowException(int x, int y) => throw new SampleException();
}
