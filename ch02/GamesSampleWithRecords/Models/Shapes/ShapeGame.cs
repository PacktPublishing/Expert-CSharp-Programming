namespace GamesSample.Models;

public class ShapeGame : Game<ShapeField, ShapeResult>
{
    internal ShapeGame(string gameType, string playerName, int numberFields, ShapeField[] solution)
        : base(gameType, playerName, numberFields, solution)
    {
    }

    protected override ShapeResult GetMoveResult(ShapeField[] guesses)
    {
        if (guesses.Count() != NumberFields)
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
