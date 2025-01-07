

using System.Text.Json.Serialization;

namespace GameModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(ColorGame), nameof(ColorGame))]
[JsonDerivedType(typeof(ShapeAndColorGame), nameof(ShapeAndColorGame))]
public abstract class Game(string playerName)
{
    public string PlayerName { get; private set; } = playerName;
    public DateTime StartTime { get; private set; } = DateTime.Now;
    public DateTime? EndTime { get; private set; }
    public bool IsWon { get; private set; } = false;
    public int LastMoveNumber { get; private set; }

    public List<Move> Moves { get; set; } = [];  // public set accessor required for JSON deserialization
    protected virtual IResult SetMove(Move move)
    {
        Moves.Add(move);
        IResult result = GetResult(move);
        if (result.IsWon)
        {
            IsWon = true;
            EndTime = DateTime.Now;
        }
        LastMoveNumber++;
        return result;
    }

    protected abstract IResult GetResult(Move move);
}

public class ColorGame(string playerName, string[] solution) : Game(playerName)
{
    public string[] Solution { get; private set; } = solution;

    public ColorResult SetMove(ColorMove move)
    {
        return (ColorResult)base.SetMove(move);
    }

    // dummy implementation, needs an algorithm to calculate the result
    // which is best placed in a separate class
    protected override IResult GetResult(Move move)
    {
        return new ColorResult(0, 0, IsWon: false);
    }
}

public class ShapeAndColorGame(string playerName, ShapeAndColor[] solution) : Game(playerName)
{
    public ShapeAndColor[] Solution { get; private set; } = solution;

    public ShapeAndColorResult SetMove(ShapeAndColorMove move)
    {
        return (ShapeAndColorResult)base.SetMove(move);
    }
    // dummy implementation
    protected override IResult GetResult(Move move)
    {
        return new ShapeAndColorResult(0, 0, 0, IsWon: false);
    }
}
