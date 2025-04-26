namespace CollectionTypes;

public partial record class Racer (string FirstName, string LastName, RacerId RacerId)
{
       public override string ToString() => $"{RacerId} - {FirstName} {LastName}";
}

public readonly partial record struct RacerId(string Prefix, int Id)
{
    public override string ToString() => $"{Prefix}-{Id}";
}