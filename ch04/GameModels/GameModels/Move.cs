using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GameModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(ColorMove), nameof(ColorMove))]
[JsonDerivedType(typeof(ShapeAndColorMove), nameof(ShapeAndColorMove))]
public abstract record class Move(int MoveNumber)
{
    //[JsonIgnore]
    //public required Game Game { get; set; }
    public int MoveId { get; private init; }
    public DateTime MoveTime { get; init; } = DateTime.Now;
}

public record class ColorMove(int MoveNumber) : Move(MoveNumber)
{
    public required string[] Colors { get; init; }
}

public record class ShapeAndColorMove(int MoveNumber) : Move(MoveNumber)
{
    public required ShapeAndColor[] ShapesAndColors { get; init; }
}

[ComplexType]
public record class ShapeAndColor(string Shape, string Color) : IParsable<ShapeAndColor>
{
    public static implicit operator ShapeAndColor((string Shape, string Color) pair) => new(pair.Shape, pair.Color); // simpler initialization

    public override string ToString() => $"{Shape}:{Color}";

    public static ShapeAndColor Parse(string s, IFormatProvider? provider = default)
    {
        if (TryParse(s, provider, out ShapeAndColor? result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException(s, nameof(s));
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ShapeAndColor result)
    {
        if (s is null)
        {
            result = default;
            return false;
        }
        string[] values = s.Split(':');
        result = new ShapeAndColor(values[0], values[1]);
        return true;
    }
}