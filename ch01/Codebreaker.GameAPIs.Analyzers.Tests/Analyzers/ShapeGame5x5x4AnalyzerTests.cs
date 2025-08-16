using System.Collections;

using static Codebreaker.GameAPIs.Models.Colors;
using static Codebreaker.GameAPIs.Models.Shapes;

namespace Codebreaker.GameAPIs.Analyzer.Tests;

public class ShapeGame5x5x4AnalyzerTests
{
    [Fact]
    public void SetMove_ShouldReturnOneWhiteAndJustOneBlueBecauseOfWhite()
    {
        ShapeAndColorResult expectedKeyPegs = new(0, 1, 1);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Square;Green", "Square;Purple", "Circle;Yellow", "Triangle;Blue"],
            ["Triangle;Green", "Circle;Yellow", "Star;Yellow", "Circle;Red"]
        );
        // Position 2: pair ok, wrong position (white)
        // Position 1: Green is correct (blue)
        // Position 3: Yellow is correct, but the position 2 already has a white for circle/yellow, thus not another blue is added!
        // Position 4: all wrong

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldNotReturnBlueWithOneWhite()
    {
        ShapeAndColorResult expectedKeyPegs = new(0, 1, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Triangle;Blue", "Square;Blue", "Triangle;Green"],
            ["Circle;Red", "Square;Green", "Triangle;Blue", "Star;Yellow"]
        );
        // Position 3: Triangle.Blue is correct, but should be in the 2nd position
        // all the other pegs are incorrect

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnOneWhiteAndOneBlue()
    {
        ShapeAndColorResult expectedKeyPegs = new(0, 1, 1);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Purple", "Square;Purple", "Star;Red", "Circle;Yellow"],
            ["Triangle;Purple", "Star;Green", "Circle;Blue", "Square;Purple"]
        );
        // position 4: Square;Purple is correct but in an incorrect position (should be 2) - white
        // position 1: Purple correct - blue

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnTwoBlack()
    {
        ShapeAndColorResult expectedKeyPegs = new(2, 0, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Rectangle;Green", "Circle;Yellow"],
            ["Rectangle;Green", "Circle;Yellow", "Star;Blue", "Star;Blue"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnOneBlackWithMultipleCorrectCodes()
    {
        ShapeAndColorResult expectedKeyPegs = new(1, 0, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Rectangle;Green", "Rectangle;Green", "Rectangle;Green"],
            ["Rectangle;Green", "Star;Blue", "Star;Blue", "Star;Blue"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnOneBlackWithMultipleCorrectPairGuesses()
    {
        ShapeAndColorResult expectedKeyPegs = new(1, 0, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Circle;Yellow", "Circle;Yellow"],
            ["Rectangle;Green", "Rectangle;Green", "Rectangle;Green", "Rectangle;Green"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnThreeWhite()
    {
        ShapeAndColorResult expectedKeyPegs = new(0, 3, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Rectangle;Green", "Star;Blue"],
            ["Circle;Yellow", "Rectangle;Green", "Star;Blue", "Square;Purple"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnOneWhiteWithMultipleCorrectPairIsGuesses()
    {
        ShapeAndColorResult expectedKeyPegs = new(0, 1, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Circle;Yellow", "Circle;Yellow"],
            ["Triangle;Blue", "Rectangle;Green", "Rectangle;Green", "Rectangle;Green"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnTwoBlueForMatchingColors()
    {
        // the second and third guess have a correct color in the correct position
        // all the shapes are incorrect
        ShapeAndColorResult expectedKeyPegs = new(0, 0, 2);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Rectangle;Green", "Circle;Yellow"],
            ["Star;Blue", "Star;Yellow", "Star;Green", "Star;Blue"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnTwoBlueForMatchingShapesAndColors()
    {
        // the first guess has a correct shape, and the second guess a correct color. All other guesses are wrong.
        ShapeAndColorResult expectedKeyPegs = new(0, 0, 2);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Rectangle;Green", "Circle;Yellow"],
            ["Rectangle;Blue", "Rectangle;Yellow", "Star;Blue", "Star;Blue"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnTwoBlueForMatchingShapes()
    {
        // the first and second guess have a correct shape, but a wrong color
        // all the colors are incorrect
        ShapeAndColorResult expectedKeyPegs = new(0, 0, 2);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Green", "Circle;Yellow", "Rectangle;Green", "Circle;Yellow"],
            ["Rectangle;Blue", "Circle;Blue", "Star;Blue", "Star;Blue"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    [Fact]
    public void SetMove_ShouldReturnOneBlackAndOneWhite()
    {
        // the first and second guess have a correct shape, but both in the wrong position
        // all the colors are incorrect
        ShapeAndColorResult expectedKeyPegs = new(1, 1, 0);
        ShapeAndColorResult? resultKeyPegs = AnalyzeGame(
            ["Rectangle;Blue", "Circle;Yellow", "Star;Green", "Circle;Yellow"],
            ["Rectangle;Blue", "Star;Green", "Triangle;Red", "Triangle;Red"]
        );

        Assert.Equal(expectedKeyPegs, resultKeyPegs);
    }

    private static MockShapeGame CreateGame(string[] codes) =>
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

    private static ShapeAndColorResult AnalyzeGame(string[] codes, string[] guesses)
    {
        MockShapeGame game = CreateGame(codes);

        ShapeGameGuessAnalyzer analyzer = new(game, guesses.ToPegs<ShapeAndColorField>().ToArray(), 1);
        return analyzer.GetResult();
    }

}

public class TestData5x5x4 : IEnumerable<object[]>
{
    public static readonly string[] Colors5 = [Red, Green, Blue, Yellow, Purple];
    public static readonly string[] Shapes5 = [Circle, Square, Triangle, Star, Rectangle];

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new string[] { Green, Blue,  Green, Yellow }, // code
            new string[] { Green, Green, Black, White },  // inputdata
            new ColorResult(1, 1) // expected
        };
        yield return new object[]
        {
            new string[] { Red,   Blue,  Black, White },
            new string[] { Black, Black, Red,   Yellow },
            new ColorResult(0, 2)
        };
        yield return new object[]
        {
            new string[] { Yellow, Black, Yellow, Green },
            new string[] { Black,  Black, Black,  Black },
            new ColorResult(1, 0)
        };
        yield return new object[]
        {
            new string[] { Yellow, Yellow, White, Red },
            new string[] { Green,  Yellow, White, Red },
            new ColorResult(3, 0)
        };
        yield return new object[]
        {
            new string[] { White, Black, Yellow, Black },
            new string[] { Black, Blue,  Black,  White },
            new ColorResult(0, 3)
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
