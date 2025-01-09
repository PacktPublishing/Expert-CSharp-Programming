namespace InterfaceVersioning;

internal class RegularCustomer(string name, DateTime joined) : ICustomer
{
    public string Name { get; } = name;

    public DateTime Joined { get; } = joined;

    public double Discount() => 0.0;
}
