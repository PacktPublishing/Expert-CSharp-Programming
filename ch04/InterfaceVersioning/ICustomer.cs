namespace InterfaceVersioning;

public interface ICustomer
{
    string Name { get; }
    DateTime Joined { get; }
    internal double Discount()
    {
        TimeSpan span = DateTime.Now - Joined;
        return span.Days switch
        {
            < 365 => 0.0,
            < 730 => 0.1,
            < 1460 => 0.2,
            _ => 0.25
        };
    }
}
