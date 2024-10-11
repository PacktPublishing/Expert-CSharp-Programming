// Game type 1: 6 colors and 4 fields
public class Game6x4 : IGame<string, (int correctPosition, int incorrectPosition)>
{
    private readonly string[] _colors = ["Red", "Blue", "Green", "Yellow", "Purple", "Orange" ];
    private readonly int _fields = 4;
    private List<string>? _solution;

    public void StartGame()
    {
        Console.WriteLine("Starting Game Type 1 with 6 colors and 4 fields.");
        _solution = GenerateSolution();
    }

    public (int correctPosition, int incorrectPosition) SetMove(string[] guesses)
    {
        if (_solution == null) throw new Exception("Game has not started yet.");

        if (guesses.Count() != _fields)
        {
            throw new ArgumentException($"Move must have exactly {_fields} fields.");
        }

        int correctPosition = 0;
        int incorrectPosition = 0;

        for (int i = 0; i < _fields; i++)
        {
            if (guesses[i] == _solution[i])
            {
                correctPosition++;
            }
            else if (_solution.Contains(guesses[i]))
            {
                incorrectPosition++;
            }
        }

        Console.WriteLine($"Game Type 1: Correct Position - {correctPosition}, Incorrect Position - {incorrectPosition}");
        return (correctPosition, incorrectPosition);
    }

    private List<string> GenerateSolution()
    {
        return [.. Random.Shared.GetItems(_colors, _fields)];
    }
}

