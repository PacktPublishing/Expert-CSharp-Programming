using Delegates;

Func<int, int, int> add = Calculator.Add;
Console.WriteLine(add(10, 5));

Func<int, int, int> subtract = (x, y) => x - y;
Console.WriteLine(subtract(10, 5));

MathOp multiply = (x, y) => x * y;
Console.WriteLine(multiply(10, 5));

Action<int, int> calcOutput = CalculationOutput.Addition;
calcOutput += CalculationOutput.Subtraction;
calcOutput(7, 2); // calcOutput.Invoke(7, 2);

Action<int, int> combinedDelegate = (Action<int, int>)Delegate.Combine(CalculationOutput.Addition, CalculationOutput.Subtraction);
combinedDelegate(11, 22);  // combinedDelegate.Invoke(11, 22);

Action<int, int> combinedWithExceptionDelegate = (Action<int, int>)Delegate.Combine(combinedDelegate, CalculationOutput.ThrowException);


foreach (Action<int, int> m in combinedWithExceptionDelegate.GetInvocationList())
{
    try
    {
        m(11, 22);
    }
    catch (SampleException)
    {
        Console.WriteLine("Exception caught, but continue with others");
    }
}
