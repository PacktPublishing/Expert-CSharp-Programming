// Base interface for a game
public interface IGame<TGuess, TResult>
{
    void StartGame();
    TResult SetMove(TGuess[] move);
}

