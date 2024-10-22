namespace LiveShaping;

public record class Racer
{
    public required string Name { get; set; }
    public required string Team { get; set; }
    public int Number { get; set; }

    public override string ToString() => Name;
}
