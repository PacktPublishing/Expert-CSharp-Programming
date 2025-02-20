using Delegates;

Func<int, int, int> add = Calculator.Add;
Console.WriteLine(add(10, 5));

Func<int, int, int> subtract = (x, y) => x - y;
Console.WriteLine(subtract(10, 5));

MathOp multiply = (x, y) => x * y;
Console.WriteLine(multiply(10, 5));

Action<int, int> calcOutput = CalculationOutput.Addition;
calcOutput += CalculationOutput.Subtraction;
calcOutput.Invoke(7, 2);

Action<int, int> combinedDelegate = (Action<int, int>)Delegate.Combine(CalculationOutput.Add, CalculationOutput.Subtraction);
combinedDelegate.Invoke(11, 22);

var delegates = combinedDelegate.GetInvocationList();

