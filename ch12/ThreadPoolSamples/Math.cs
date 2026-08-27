namespace ThreadPoolSamples;

internal class Math
{

    /// <summary>
    /// Calculates the nth Fibonacci number.
    /// </summary>
    /// <param name="n">The zero-based position in the Fibonacci sequence.</param>
    /// <returns>The Fibonacci number at the specified position.</returns>
    public static int Fibonacci(int n)
    {
        if (n <= 1)
            return n;

        int a = 0, b = 1;

        for (int i = 2; i <= n; i++)
        {
            (a, b) = (b, a + b);
        }

        return b;
    }
}
