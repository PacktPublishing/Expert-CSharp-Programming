using System.Collections;

IEnumerable<int> data = [1, 2, 3, 4, 5];

foreach (var item in data)
{
    Console.WriteLine(item);
}

using IEnumerator<int> enumerator = data.GetEnumerator();
while (enumerator.MoveNext())
{
    Console.WriteLine(enumerator.Current);
}

IEnumerable<int>? data1 = null;
var enumerator1 = CustomImplementation.CustomEnumerator.Where<int>(null!, null!);

Console.WriteLine("this statement runs!");

foreach (int x in enumerator1)
{
    Console.WriteLine(x);
}

// TODO: show deferred invocation



static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> predicate)
{
    foreach (var item in source)
    {
        if (predicate(item))
        {
            yield return item;
        }
    }
}

public class Enumerable<TSource, TResult>(
    IEnumerable<TSource> source,
    Func<TSource, TResult> predicate)
    : IEnumerable<TResult>
{
    public IEnumerator<TResult> GetEnumerator() =>
        new FilterEnumerator<TSource, TResult>(source, predicate);

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}

public sealed class FilterEnumerator<TSource, TResult>(
    IEnumerable<TSource> source,
    Func<TSource, TResult> predicate) :
    IEnumerator<TResult>
{
    private IEnumerator<TSource>? _enumerator;
    private int _state = 1;

    public TResult Current
    { get; private set; } = default!;
    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        _enumerator?.Dispose();
    }

    public bool MoveNext()
    {
        switch (_state)
        {
            case 1:
                _enumerator = source.GetEnumerator();
                _state = 2;
                goto case 2; // Fall through to the next case
            case 2:
                try
                {
                    if (_enumerator?.MoveNext() ?? false)
                    {
                        Current = predicate(_enumerator.Current);
                        return true;
                    }
                }
                catch
                {
                    Dispose();
                    throw;
                }
                break;
        }

        Dispose();
        return false;
    }

    //public bool MoveNext()
    //{
    //    _enumerator = source.GetEnumerator();
    //    try
    //    {
    //        if (_enumerator.MoveNext())
    //        {
    //            Current = predicate(_enumerator.Current);
    //            return true;
    //        }
    //    }
    //    catch
    //    {
    //        Dispose();
    //        throw;
    //    }
    //    return false;
    //}

    public void Reset()
    {
        // Legacy from COM
        throw new NotSupportedException(); // Most implementations do not support Reset
    }
}

//public class Enumerable<TSource, TResult>(
//    IEnumerable<TSource> source, 
//    Func<TSource, TResult> predicate) 
//    : IEnumerable<TResult>
//{
//    public IEnumerator<TResult> GetEnumerator() => 
//        new FilterEnumerator<TSource, TResult>(source, predicate);

//    IEnumerator IEnumerable.GetEnumerator() => 
//        GetEnumerator();
//}

//public sealed class FilterEnumerator<TSource, TResult>(
//    IEnumerable<TSource> source, 
//    Func<TSource, TResult> predicate) : 
//    IEnumerator<TResult>
//{
//    private IEnumerator<TSource>? _enumerator;
//    private int _state = 1;

//    public TResult Current
//    { get; private set; } = default!;
//    object? IEnumerator.Current => Current;

//    public void Dispose()
//    {
//        _enumerator?.Dispose();
//    }

//    public bool MoveNext()
//    {
//        switch (_state)
//        {
//            case 1:
//                _enumerator = source.GetEnumerator();
//                _state = 2;
//                goto case 2; // Fall through to the next case
//            case 2:
//                try
//                {
//                    if (_enumerator?.MoveNext() ?? false)
//                    {
//                        Current = predicate(_enumerator.Current);
//                        return true;
//                    }
//                }
//                catch
//                {
//                    Dispose();
//                    throw;
//                }
//                break;
//        }

//        Dispose();
//        return false;
//    }

//    //public bool MoveNext()
//    //{
//    //    _enumerator = source.GetEnumerator();
//    //    try
//    //    {
//    //        if (_enumerator.MoveNext())
//    //        {
//    //            Current = predicate(_enumerator.Current);
//    //            return true;
//    //        }
//    //    }
//    //    catch
//    //    {
//    //        Dispose();
//    //        throw;
//    //    }
//    //    return false;
//    //}

//    public void Reset()
//    {
//        // Legacy from COM
//        throw new NotSupportedException(); // Most implementations do not support Reset
//    }
//}