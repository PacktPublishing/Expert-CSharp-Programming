// Game type 3: 5 colors, 5 shapes, and 4 fields
public class Game5x5x4 : IGame<(string color, string shape), ShapeGameResult>
{
    private readonly string[] _colors = ["Red", "Blue", "Green", "Yellow", "Purple"];
    private readonly string[] _shapes = [ "Circle", "Square", "Triangle", "Hexagon", "Star" ];
    private readonly int _fields = 4;
    private List<(string color, string shape)>? _solution;

    public void StartGame()
    {
        Console.WriteLine("Starting Game Type 3 with 5 colors, 5 shapes, and 4 fields.");
        _solution = GenerateSolution();
    }

    public ShapeGameResult SetMove((string color, string shape)[] guesses)
    {
        if (_solution == null) throw new Exception("Game has not started yet.");

        if (guesses.Count() != _fields)
        {
            throw new ArgumentException($"Move must have exactly {_fields} fields.");
        }

        int correctPosition = 0;
        int incorrectPosition = 0;
        int partialCorrect = 0;

        for (int i = 0; i < _fields; i++)
        {
            if (guesses[i] == _solution[i])
            {
                correctPosition++;
            }
            else if (_solution.Any(s => s.color == guesses[i].color && s.shape == guesses[i].shape))
            {
                incorrectPosition++;
            }
            else if (_solution.Any(s => s.color == guesses[i].color || s.shape == guesses[i].shape))
            {
                partialCorrect++;
            }
        }

        Console.WriteLine($"Game Type 3: Correct Position - {correctPosition}, Incorrect Position - {incorrectPosition}, Partial Correct - {partialCorrect}");
        return new ShapeGameResult(correctPosition, incorrectPosition, partialCorrect);
    }

    private List<(string color, string shape)> GenerateSolution()
    {
        var colors = Random.Shared.GetItems(_colors, _fields);
        var shapes = Random.Shared.GetItems(_shapes, _fields);
        return [.. colors.Zip(shapes).Select(x => (x.First, x.Second)) ];
    }
}

public readonly record struct ShapeGameResult(int CorrectPosition, int IncorrectPosition, int PartialCorrect);

