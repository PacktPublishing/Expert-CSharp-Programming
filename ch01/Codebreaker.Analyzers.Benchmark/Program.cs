using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using Codebreaker.GameAPIs.Analyzers;
using Codebreaker.GameAPIs.Contracts;
using Codebreaker.GameAPIs.Extensions;
using Codebreaker.GameAPIs.Models;

using static Codebreaker.GameAPIs.Models.Colors;
using static Codebreaker.GameAPIs.Models.Shapes;

BenchmarkRunner.Run<BenchmarkAnalyzer>();

public class Game : IGame
{
    public Guid Id { get; init; }
    public int NumberCodes { get; init; }
    public int MaxMoves { get; init; }
    public DateTime? EndTime { get; set; }
    public bool IsVictory { get; set; }

    public required IDictionary<string, IEnumerable<string>> FieldValues { get; init; }
    public required string[] Codes { get; init; }

    public DateTime StartTime { get; }
    public TimeSpan? Duration { get; set; }
    public int LastMoveNumber { get; set; }
    public required string GameType { get; init; }
}

public class TestData5x5x4
{
    public static readonly string[] Colors5 = [Red, Green, Blue, Yellow, Purple];
    public static readonly string[] Shapes5 = [Circle, Square, Triangle, Star, Rectangle];
}

public class TestData8x5
{
    public static readonly string[] Colors8 = [Red, Blue, Green, Yellow, Black, White, Purple, Orange];
}

public class TestData6x4
{
    public static readonly string[] Colors6 = [Red, Green, Blue, Yellow, Black, White];
}    

[MemoryDiagnoser]
public class BenchmarkAnalyzer
{
    private static Game Create5x5x4Game(string[] codes) =>
        new()
        {
            GameType = GameTypes.Game5x5x4,
            NumberCodes = 4,
            MaxMoves = 14,
            IsVictory = false,
            FieldValues = new Dictionary<string, IEnumerable<string>>()
            {
                [FieldCategories.Colors] = TestData5x5x4.Colors5.ToList(),
                [FieldCategories.Shapes] = TestData5x5x4.Shapes5.ToList()
            },
            Codes = codes
        };

    private static Game Create8x5Game(string[] codes) =>
    new()
    {
        GameType = GameTypes.Game8x5,
        NumberCodes = 5,
        MaxMoves = 12,
        IsVictory = false,
        FieldValues = new Dictionary<string, IEnumerable<string>>()
        {
            [FieldCategories.Colors] = TestData8x5.Colors8.ToList()
        },
        Codes = codes
    };

    private static Game Create6x4Game(string[] codes) =>
    new()
    {
        GameType = GameTypes.Game6x4,
        NumberCodes = 4,
        MaxMoves = 12,
        IsVictory = false,
        FieldValues = new Dictionary<string, IEnumerable<string>>()
        {
            [FieldCategories.Colors] = TestData6x4.Colors6.ToList()
        },
        Codes = codes
    };

    private static ShapeAndColorResult Analyze5x5x4Game(string[] codes, string[] guesses)
    {
        Game game = Create5x5x4Game(codes);

        ShapeGameGuessAnalyzer analyzer = new(game, guesses.ToPegs<ShapeAndColorField>().ToArray(), 1);
        return analyzer.GetResult();
    }

    private static ColorResult Analyze8x5Game(string[] codes, string[] guesses)
    {
        Game game = Create8x5Game(codes);

        ColorGameGuessAnalyzer analyzer = new(game, guesses.ToPegs<ColorField>().ToArray(), 1);
        return analyzer.GetResult();
    }

    private static ColorResult Analyze6x4Game(string[] codes, string[] guesses)
    {
        Game game = Create6x4Game(codes);

        ColorGameGuessAnalyzer analyzer = new(game, guesses.ToPegs<ColorField>().ToArray(), 1);
        return analyzer.GetResult();
    }

    private string[] codes5x5x4 = ["Square;Green", "Square;Purple", "Circle;Yellow", "Triangle;Blue"];
    private string[] guesses5x5x4 = ["Triangle;Green", "Circle;Yellow", "Star;Yellow", "Circle;Red"];

    private string[] codes8x5 = ["Green", "Purple", "Yellow", "Blue", "Red"];
    private string[] guesses8x5 = ["Green", "Yellow", "Yellow", "Red", "Blue"];

    private string[] codes6x4 = ["Green", "Purple", "Yellow", "Blue"];
    private string[] guesses6x4 = ["Green", "Yellow", "Yellow", "Red"];


    [Benchmark]
    public void Analyze5x5x4Game() => Analyze5x5x4Game(codes5x5x4, guesses5x5x4);

    [Benchmark]
    public void Analyze8x5Game() => Analyze8x5Game(codes8x5, guesses8x5);

    [Benchmark]
    public void Analyze6x4Game() => Analyze6x4Game(codes6x4, guesses6x4);
}