

using System.Text.Json.Serialization;

namespace GameModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
[JsonDerivedType(typeof(ColorGame), nameof(ColorGame))]
[JsonDerivedType(typeof(ShapeAndColorGame), nameof(ShapeAndColorGame))]
public abstract class Game(string playerName, int gameId = 0)
{
    public int GameId { get; private set; } = gameId;
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

// constructor cannot be used with EF Core
// public class ColorGame(string playerName, string[] solution, int gameId = 0) : Game(playerName, gameId)
public class ColorGame(string playerName, int gameId = 0) : Game(playerName, gameId)
{
    public required string[] Solution { get; set; }

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

public class ShapeAndColorGame(string playerName, int gameId = 0) : Game(playerName, gameId)
{
    public required ShapeAndColor[] Solution { get; set; }

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
