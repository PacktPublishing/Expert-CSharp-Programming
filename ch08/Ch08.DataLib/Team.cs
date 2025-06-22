namespace Ch08.DataLib;

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Country { get; set; }
    public int FoundedYear { get; set; }
    public ICollection<Racer> Racers { get; set; } = [];

    public override string ToString() => Name;
}