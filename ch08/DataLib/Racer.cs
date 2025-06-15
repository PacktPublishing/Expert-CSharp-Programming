namespace DataLib;

public record class Racer(
    string FirstName, 
    string LastName, 
    string Country, 
    DateOnly DayOfBirth, 
    DateOnly? DayOfDeath = default,
    int Wins = 0,
    int PolePositions = 0)
{
    public IList<Team> Teams { get; internal set; } = [];
    public IEnumerable<int> Championships { get; internal set; } = [];
}

