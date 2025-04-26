namespace Sorting;

public partial record class Racer (string FirstName, string LastName, RacerId RacerId)
{
    public Racer(string firstName, string lastName) : this(firstName, lastName, RacerId.Empty) { }

    public override string ToString() => $"{RacerId} - {FirstName} {LastName}";
}

public readonly partial record struct RacerId(string Prefix, int Id)
{
    public static RacerId Empty => new(string.Empty, 0);
    public override string ToString() => $"{Prefix}-{Id}";
}