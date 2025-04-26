namespace Data;

public static class ComparisonExtensions
{
    extension(int primaryComparison)
    {
        public int ThenBy(Func<int> secondaryComparison) =>
            primaryComparison != 0 ? primaryComparison : secondaryComparison();
    }
}
