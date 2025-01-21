namespace GamesSample.Models;
#if USERECORDS
public class ShapeGame : Game<ShapeField, ShapeResult>
{
    internal ShapeGame(string gameType, string playerName, int numberFields, ShapeField[] solution)
        : base(gameType, playerName, numberFields, solution)
    {
    }

    protected override ShapeResult GetMoveResult(ShapeField[] guesses)
    {
        if (guesses.Length != NumberFields)
        {
            throw new ArgumentException($"Guesses must have exactly {NumberFields} fields.");
        }

        int correctPosition = 0;
        int incorrectPosition = 0;
        int partialCorrect = 0;

        for (int i = 0; i < NumberFields; i++)
        {
            if (guesses[i] == Solution[i])
            {
                correctPosition++;
            }
            else if (Solution.Any(s => s.Color == guesses[i].Color && s.Shape == guesses[i].Shape))
            {
                incorrectPosition++;
            }
            else if (Solution.Any(s => s.Color == guesses[i].Color || s.Shape == guesses[i].Shape))
            {
                partialCorrect++;
            }
        }

        return new (correctPosition, incorrectPosition, partialCorrect);
    }
}
#else
public class ShapeGame : Game<(string Color, string Shape), (int CorrectPosition, int IncorrectPosition, int PartlyCorrect)>
{
    internal ShapeGame(string gameType, string playerName, int numberFields, (string Color, string Shape)[] solution)
        : base(gameType, playerName, numberFields, solution)
    {
    }

    protected override (int CorrectPosition, int IncorrectPosition, int PartlyCorrect) GetMoveResult((string Color, string Shape)[] guesses)
    {
        if (guesses.Length != NumberFields)
        {
            throw new ArgumentException($"Guesses must have exactly {NumberFields} fields.");
        }

        int correctPosition = 0;
        int incorrectPosition = 0;
        int partialCorrect = 0;


        for (int i = 0; i < NumberFields; i++)
        {
            if (guesses[i] == Solution[i])
            {
                correctPosition++;
            }
            else if (Solution.Any(s => s.Color == guesses[i].Color && s.Shape == guesses[i].Shape))
            {
                incorrectPosition++;
            }
            else if (Solution.Any(s => s.Color == guesses[i].Color || s.Shape == guesses[i].Shape))
            {
                partialCorrect++;
            }
        }

        return (correctPosition, incorrectPosition, partialCorrect);
    }
}
#endif