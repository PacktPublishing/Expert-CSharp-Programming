namespace GamesSample.Models;

public interface IGame
{
    Guid GameId { get; }
    string GameType { get; }
    string PlayerName { get; }
    int NumberFields { get; }
}

public interface IGame<TField, TResult> : IGame
    where TResult : struct
{
    TField[] Solution { get; }
    IEnumerable<Move<TField, TResult>> Moves { get; }
    TResult AddMove(TField[] guesses);

    static Type Field => typeof(TField);
    static Type Result => typeof(TResult);
}

public readonly record struct Move<TField, TResult>(TField[] Guesses, TResult Result);
