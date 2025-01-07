namespace GameModels;

public interface IResult 
{ 
    bool IsWon { get; } 
};

public readonly record struct ColorResult(int CorrectColorPositions, int CorrectColors, bool IsWon) : IResult;

public readonly record struct ShapeAndColorResult(int CorrectPairPositions, int CorrectPair, int CorrectColorOrShapes, bool IsWon) : IResult;