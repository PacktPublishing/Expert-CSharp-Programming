using System.Numerics;

public static class Calculations
{
    public static TSelf Sum<TSelf>(this IEnumerable<TSelf> values)
        where TSelf : INumberBase<TSelf>
    {
        TSelf result = TSelf.Zero;
        foreach (var value in values)
        {
            result += value;
        }
        return result;
    }

    public static TResult Sum<TSelf, TResult>(this IEnumerable<TSelf> values)
        where TSelf : INumberBase<TSelf>
        where TResult : INumberBase<TResult>
    {
        TResult result = TResult.Zero;
        foreach (var value in values)
        {
            TResult.CreateChecked(value);
            result += TResult.CreateChecked(value);
        }

        return result;
    }

    public static TResult Average<TSelf, TResult>(this IEnumerable<TSelf> values)
        where TSelf : INumberBase<TSelf>
        where TResult : INumberBase<TResult>
    {
        TResult sum = Sum<TSelf, TResult>(values);
        return TResult.CreateChecked(sum) / TResult.CreateChecked(values.Count());
    }
}

