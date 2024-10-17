
namespace GamesSample.Models;

// not using primary constructor because of internal access modifier
public class ColorGame : Game<string, (int CorrectPosition, int IncorrectPosition)>
{
    internal ColorGame(string gameType, string playerName, int numberFields, string[] solution)
        : base(gameType, playerName, numberFields, solution)
    {
    }

    protected override (int CorrectPosition, int IncorrectPosition) GetMoveResult(string[] guesses)
    {
        if (guesses.Count() != NumberFields)
        {
            throw new ArgumentException($"Guesses must have exactly {NumberFields} fields.");
        }

        int correctPosition = 0;
        int incorrectPosition = 0;

        for (int i = 0; i < NumberFields; i++)
        {
            if (guesses[i] == Solution[i])  // this is a match for the color and the position
            {
                correctPosition++;
            }
            else if (Solution.Contains(guesses[i])) // this is a match just for the color
            {
                incorrectPosition++;
            }
        }

        return (correctPosition, incorrectPosition);
    }
}
