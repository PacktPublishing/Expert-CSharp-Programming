using System.Collections;

using static Codebreaker.GameAPIs.Models.Colors;

namespace Codebreaker.GameAPIs.Analyzer.Tests;

public class ColorGame8x5AnalyzerTests
{
    [Fact]
    public void SetMove_ShouldReturnThreeWhite()
    {
        ColorResult expectedKeyPegs = new(0, 3);
        ColorResult? resultKeyPegs = AnalyzeGame(
            [Green, Yellow, Green, Black, Orange],
            [Yellow, Green, Black, Blue, Blue]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [InlineData(1, 2, Red, Yellow, Red, Blue, Orange)]
    [InlineData(2, 0, White, White, Blue, Red, White)]
    [Theory]
    public void SetMove_UsingVariousData(int expectedBlack, int expectedWhite, params string[] guessValues)
    {
        string[] code = [Red, Green, Blue, Red, Brown];
        ColorResult expectedKeyPegs = new (expectedBlack, expectedWhite);
        ColorResult resultKeyPegs = AnalyzeGame(code, guessValues);
        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Theory]
    [ClassData(typeof(TestData8x5))]
    public void SetMove_UsingVariousDataUsingDataClass(string[] code, string[] guess, ColorResult expectedKeyPegs)
    {
        ColorResult actualKeyPegs = AnalyzeGame(code, guess);
        Assert.Equal(expectedKeyPegs, actualKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldThrowOnInvalidGuessCount()
    {
        Assert.Throws<ArgumentException>(() => 
            AnalyzeGame(
                ["Black", "Black", "Black", "Black", "Black"],
                ["Black"]
            ));
    }

    [Fact]
    public void SetMove_ShouldThrowOnInvalidGuessValues()
    {
        Assert.Throws<ArgumentException>(() => 
            AnalyzeGame(
                ["Black", "Black", "Black", "Black", "Black"],
                ["Black", "Der", "Blue", "Yellow", "Black"]      // "Der" is the wrong value
            ));
    }

    private static MockColorGame CreateGame(string[] codes) => 
        new()
        {
            GameType = GameTypes.Game8x5,
            NumberCodes = 5,
            MaxMoves = 14,
            IsVictory = false,
            FieldValues = new Dictionary<string, IEnumerable<string>>()
            {
                [FieldCategories.Colors] = TestData8x5.Colors8.ToList()
            },
            Codes = codes
        };

    private static ColorResult AnalyzeGame(string[] codes, string[] guesses)
    {
        MockColorGame game = CreateGame(codes);

        ColorGameGuessAnalyzer analyzer = new(game, guesses.ToPegs<ColorField>().ToArray(), 1);
        return analyzer.GetResult();
    }
}

public class TestData8x5 : IEnumerable<object[]>
{
    public static readonly string[] Colors8 = [Red, Blue, Green, Yellow, Black, White, Purple, Orange];

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new string[] { Green, Blue,  Green, Yellow, Orange }, // code
            new string[] { Green, Green, Black, White, Black },  // inputdata
            new ColorResult(1, 1) // expected
        };
        yield return new object[]
        {
            new string[] { Red, Blue, Black, White, Orange },
            new string[] { Black, Black, Red, Yellow, Yellow },
            new ColorResult(0, 2)
        };
        yield return new object[]
        {
            new string[] { Yellow, Black, Yellow, Green, Orange },
            new string[] { Black,  Black, Black,  Black, Black },
            new ColorResult(1, 0)
        };
        yield return new object[]
        {
            new string[] { Yellow, Yellow, White, Red, Orange },
            new string[] { Green,  Yellow, White, Red, Green },
            new ColorResult(3, 0)
        };
        yield return new object[]
        {
            new string[] { White, Black, Yellow, Black, Orange },
            new string[] { Black, Blue,  Black,  White, White },
            new ColorResult(0, 3)
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
