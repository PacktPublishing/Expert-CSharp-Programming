namespace LiveShaping;

public record Racer
{
    public required string Name { get; init; }
    public required string Team { get; init; }
    public int Number { get; init; }

    public override string ToString() => Name;

}