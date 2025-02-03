Console.WriteLine("Enter a number");

string? input1 = Console.ReadLine();

if (int.TryParse(input1, out int number1))
{
    Console.WriteLine($"You entered {number1}");
}
else
{
    Console.WriteLine("Parsing of the input failed");
}

Console.WriteLine("Enter a number");
string input2 = Console.ReadLine() ?? 
    throw new InvalidOperationException("Null returned from Console.ReadLine()");

try
{
    int number2 = int.Parse(input2); // This will throw an exception if the input is not a number
    Console.WriteLine($"You entered {number2}");
}
catch (FormatException ex)
{
    Console.WriteLine(ex.Message);
}
catch (OverflowException ex)
{
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}