namespace LiveShaping;

public class Racer
{
    public required string Name { get; set; }
    public required string Team { get; set; }
    public int Number { get; set; }

    public override string ToString() => Name;
}
