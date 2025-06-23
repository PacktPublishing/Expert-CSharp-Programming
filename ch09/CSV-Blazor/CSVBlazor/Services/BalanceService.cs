namespace TrafficLightBlazor.Services;

public class BalanceService
{
    public double GetBalance(string[] data)
    {
        double balance = 0.0;
        foreach (var line in data)
        {
            string[] values = line.Split(", ");

            balance += values switch
            {
                [_, "DEPOSIT", _, var amount] => double.Parse(amount),
                [_, "WITHDRAW", _, var amount] => -double.Parse(amount),
                [_, "FEE", var amount] => -double.Parse(amount),
                _ => 0.0
            };
        }
        return balance;
    }

    public IDictionary<DateOnly, double> GetBalanceByMonth(string[] data)
    {
        Dictionary<DateOnly, double> monthlyBalance = [];

        foreach (var line in data)
        {
            string[] values = line.Split(", ");
            _ = values switch
            {
                [var dateStr, "DEPOSIT", _, var amt] 
                    when DateOnly.TryParse(dateStr, out var date) => 
                        AddToMonth(date, double.Parse(amt)),
                [var dateStr, "WITHDRAW", _, var amt] 
                    when DateOnly.TryParse(dateStr, out var date) => 
                        AddToMonth(date, -double.Parse(amt)),
                [var dateStr, "FEE", var amt] 
                    when DateOnly.TryParse(dateStr, out var date) => 
                    AddToMonth(date, -double.Parse(amt)),
                _ => false
            };
        }
        return monthlyBalance;

        bool AddToMonth(DateOnly date, double amount)
        {
            DateOnly monthKey = new(date.Year, date.Month, 1);
            if (monthlyBalance.ContainsKey(monthKey))
                monthlyBalance[monthKey] += amount;
            else
                monthlyBalance[monthKey] = amount;
            return true;
        }
    }
}