namespace Ch08.DataLib;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int FoundedYear { get; set; }
    public ICollection<Racer> Racers { get; set; } = new List<Racer>();

    public override string ToString() => Name;
}