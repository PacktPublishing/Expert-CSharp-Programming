// Game type with 6 colors and 4 fields
public class Game6x4 : IGame<string, (int CorrectPosition, int IncorrectPosition)>
{
    private readonly string[] _colors = ["Red", "Blue", "Green", "Yellow", "Purple", "Orange" ];
    private readonly int _fields = 4;
    private List<string>? _solution;

    public void StartGame()
    {
        Console.WriteLine("Starting Game Type with 6 colors and 4 fields.");
        _solution = GenerateSolution();
    }

    public (int CorrectPosition, int IncorrectPosition) SetMove(string[] move)
    {
        if (_solution == null) throw new Exception("Game has not started yet.");

        if (move.Count() != _fields)
        {
            throw new ArgumentException($"Move must have exactly {_fields} fields.");
        }

        int correctPosition = 0;
        int incorrectPosition = 0;

        for (int i = 0; i < _fields; i++)
        {
            if (move[i] == _solution[i])
            {
                correctPosition++;
            }
            else if (_solution.Contains(move[i]))
            {
                incorrectPosition++;
            }
        }

        Console.WriteLine($"Game Type 1: Correct Position - {correctPosition}, Incorrect Position - {incorrectPosition}");
        return (correctPosition, incorrectPosition);
    }

    private List<string> GenerateSolution() => 
        [.. Random.Shared.GetItems(_colors, _fields)];
}

