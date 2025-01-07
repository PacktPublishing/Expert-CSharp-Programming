using System.Text.Json.Serialization;

namespace GameModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(ColorMove), nameof(ColorMove))]
[JsonDerivedType(typeof(ShapeAndColorMove), nameof(ShapeAndColorMove))]
public abstract record class Move([property: JsonIgnore] Game Game, int MoveNumber)
{
    public DateTime MoveTime { get; init; } = DateTime.Now;
}

public record class ColorMove(Game Game, int MoveNumber, string[] Colors) : Move(Game, MoveNumber);

public record class ShapeAndColorMove(Game Game, int MoveNumber, ShapeAndColor[] ShapesAndColors) : Move(Game, MoveNumber);

public record struct ShapeAndColor(string Shape, string Color)
{
    public static implicit operator ShapeAndColor((string Shape, string Color) pair) => new(pair.Shape, pair.Color); // simpler initialization
    // public static implicit operator (string Shape, string Color)(ShapeAndColor pair) => (pair.Shape, pair.Color); // don't need this! 
}