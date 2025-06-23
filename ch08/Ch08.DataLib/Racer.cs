namespace Ch08.DataLib;

public class Racer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public DateOnly BirthDay { get; set; }
    public DateOnly? DayOfDeath = default;
    public int Wins { get; set; }
    public int PolePositions { get; set; }
    public List<RacerTeamMap> Teams { get; private set; } = [];
    public List<int> Championships { get; private set; } = [];

    public override string ToString() => $"{FirstName} {LastName}";
}