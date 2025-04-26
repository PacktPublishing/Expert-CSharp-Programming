namespace Data;

public partial record class Racer (string FirstName, string LastName, string Country, DateOnly BirthDay, int NumberWins)
{
    public override string ToString() => $"{FirstName} {LastName}";
}
