using System.Numerics;

// non-generic version using int
public record struct SimpleNumber(int Value) : IAdditionOperators<SimpleNumber, SimpleNumber, SimpleNumber>, ISubtractionOperators<SimpleNumber, SimpleNumber, SimpleNumber>
{
    public static SimpleNumber operator +(SimpleNumber left, SimpleNumber right) => new(left.Value + right.Value);

    public static SimpleNumber operator -(SimpleNumber left, SimpleNumber right) => new(left.Value - right.Value);
}

public record struct SimpleNumber<TValue>(TValue Value) : IAdditionOperators<SimpleNumber<TValue>, SimpleNumber<TValue>, SimpleNumber<TValue>>, ISubtractionOperators<SimpleNumber<TValue>, SimpleNumber<TValue>, SimpleNumber<TValue>>
    where TValue : struct, IAdditionOperators<TValue, TValue, TValue>, ISubtractionOperators<TValue, TValue, TValue>
{
    public static SimpleNumber<TValue> operator +(SimpleNumber<TValue> left, SimpleNumber<TValue> right) => new SimpleNumber<TValue>(left.Value + right.Value);

    public static SimpleNumber<TValue> operator -(SimpleNumber<TValue> left, SimpleNumber<TValue> right) => new SimpleNumber<TValue>(left.Value - right.Value);
}
