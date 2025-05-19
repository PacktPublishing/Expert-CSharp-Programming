namespace DataLib;

public record class Racer(string FirstName, string LastName, string Country, string TeamName, DateOnly DayOfBirth, DateOnly? DayOfDeath = default)
{
    public Dictionary<int, Team> Teams { get; } = [];
    public int Wins { get; set; }
    public int PolePositions { get; set; }
    public List<int> Championships { get; internal set; } = [];
} 

