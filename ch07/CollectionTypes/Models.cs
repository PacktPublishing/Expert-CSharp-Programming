using System.Diagnostics.CodeAnalysis;

namespace CollectionTypes;

public class SomeData(long value) : IComparable<SomeData>
{
    public long A { get; set; } = value;
    public long B { get; set; } = value;
    public long C { get; set; } = value;
    public long D { get; set; } = value;
    public long E { get; set; } = value;

    public int CompareTo(SomeData? other)
    {
        if (other is null) return 1;

        int result = A.CompareTo(other.A);
        if (result != 0) return result;

        result = B.CompareTo(other.B);
        if (result != 0) return result;

        result = C.CompareTo(other.C);
        if (result != 0) return result;

        result = D.CompareTo(other.D);
        if (result != 0) return result;

        return E.CompareTo(other.E);
    }

    public override string ToString() => $"{A}";
}

public struct SomeValue(long value)
{
    public long A { get; set; } = value;
    public long B { get; set; } = value;
    public long C { get; set; } = value;
    public long D { get; set; } = value;
    public long E { get; set; } = value;
    public override readonly string ToString() => $"{A}";
}

public record class Racer (string Name, RacerId RacerId);

public readonly partial record struct RacerId(string Prefix, int Id)
{
    public override string ToString() => $"{Prefix}-{Id}";
}
