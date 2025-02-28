using Delegates;

Func<int, int, int> add = Calculator.Add;
Console.WriteLine(add(33, 9));

Func<int, int, int> subtract = static int (x, y) => x - y;
Console.WriteLine(subtract(49, 7));

MathOp multiply = static (x, y) => x * y;
Console.WriteLine(multiply(10, 5));

CalculationOutput calculation = new(Console.Out);
Action<int, int> calcOutput = calculation.Add;
calcOutput += calculation.Subtract;
calcOutput(7, 2); // calcOutput.Invoke(7, 2);

Action<int, int> combinedDelegate = (Action<int, int>)Delegate.Combine(calculation.Add, calculation.Subtract);
combinedDelegate(11, 22);  // combinedDelegate.Invoke(11, 22);

// With the parameters of the Delegate.Combine method, it's guaranteed that the return value is not null.
Action<int, int> combinedWithExceptionDelegate = (Action<int, int>)Delegate.Combine(calculation.Add, CalculationOutput.ThrowException, calculation.Subtract)!;

// available before .NET 9
//foreach (Action<int, int> m in combinedWithExceptionDelegate.GetInvocationList())
//{
//    try
//    {
//        m(11, 22);
//    }
//    catch (SampleException)
//    {
//        Console.WriteLine("Exception caught, but continue with others");
//    }
//}

foreach (Action<int, int> m in Delegate.EnumerateInvocationList(combinedWithExceptionDelegate))
{
    try
    {
        m(55, 22);
    }
    catch (SampleException)
    {
        Console.WriteLine("Exception caught, but continue with others");
    }
}
