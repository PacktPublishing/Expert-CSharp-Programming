namespace Delegates;

internal class CalculationOutput
{
    public static void Addition(int x, int y)
    {
        Console.WriteLine($"{x} + {y} results in {x + y}");
    }

    public static void Subtraction(int x, int y)
    {
        Console.WriteLine($"{x} - {y} results in {x - y}");
    }
}
