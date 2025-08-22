
namespace GamesSample.Models;

public abstract class Game<TField, TResult>(string gameType, string playerName, int numberFields, TField[] solution) : 
    IGame<TField, TResult>
    where TResult : struct
{
    public Guid GameId { get; } = Guid.NewGuid();
    public int NumberFields { get; } = numberFields;
    public TField[] Solution { get; } = solution;

    public string GameType { get; } = gameType;

    public string PlayerName { get; } = playerName;

    public TResult AddMove(TField[] guesses)
    {
        var result = GetMoveResult(guesses);
        _moves.Add(new Move<TField, TResult>(guesses, result));
        return result;
    }

    protected abstract TResult GetMoveResult(TField[] guesses);

    private readonly List<Move<TField, TResult>> _moves = [];
    public IEnumerable<Move<TField, TResult>> Moves => _moves;
}
