namespace Ch08.DataLib;

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public int FoundedYear { get; set; }
    public ICollection<RacerTeamMap> Racers { get; set; } = [];
    public List<int> Championships { get; private set; } = [];

    public override string ToString() => Name;
}