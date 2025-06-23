Console.OutputEncoding = System.Text.Encoding.UTF8;

string[] lines = await File.ReadAllLinesAsync("data.csv");

double balance = GetBalance(lines);
Console.WriteLine("Balance");
Console.WriteLine($"{balance:C2}");
Console.WriteLine();

var monthlyBalance = GetBalanceByMonth(lines);
Console.WriteLine("Monthly balance");
foreach (var (month, amount) in monthlyBalance)
{
    Console.WriteLine($"{month:yyyy-MM}: {amount:C2}");
}

double GetBalance(string[] data)
{
    double balance = 0.0;
    foreach (var line in data)
    {
        string[] values = line.Split(", ");

        balance += values switch
        {
            [_, "DEPOSIT", _, var amount]  => double.Parse(amount),
            [_, "WITHDRAW", _, var amount] => -double.Parse(amount),
            [_, "FEE", var amount] => -double.Parse(amount),
            _ => 0.0
        };
    }
    return balance;
}

IDictionary<DateOnly, double> GetBalanceByMonth(string[] data)
{
    Dictionary<DateOnly, double> monthlyBalance = [];

    foreach (var line in data)
    {
        string[] values = line.Split(", ");
        _ = values switch
        {
            [var dateStr, "DEPOSIT", _, var amt] when DateOnly.TryParse(dateStr, out var date) => AddToMonth(date, double.Parse(amt)),
            [var dateStr, "WITHDRAW", _, var amt] when DateOnly.TryParse(dateStr, out var date) => AddToMonth(date, -double.Parse(amt)),
            [var dateStr, "FEE", var amt] when DateOnly.TryParse(dateStr, out var date) => AddToMonth(date, -double.Parse(amt)),
            _ => false
        };
    }
    return monthlyBalance;

    bool AddToMonth(DateOnly date, double amount)
    {
        DateOnly monthKey = new (date.Year, date.Month, 1);
        if (monthlyBalance.ContainsKey(monthKey))
            monthlyBalance[monthKey] += amount;
        else
            monthlyBalance[monthKey] = amount;
        return true;
    }
}