namespace InterfaceVersioning;

internal class Customer(string name, DateTime joined) : ICustomer
{
    public string Name { get; } = name;
    public DateTime Joined { get; } = joined;
}
