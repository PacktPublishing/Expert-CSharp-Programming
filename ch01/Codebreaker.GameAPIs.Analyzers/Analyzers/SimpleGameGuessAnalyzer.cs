namespace Codebreaker.GameAPIs.Analyzers;

public class SimpleGameGuessAnalyzer(IGame game, ColorField[] guesses, int moveNumber) : GameGuessAnalyzer<ColorField, SimpleColorResult>(game, guesses, moveNumber)
{
    protected override void ValidateGuessValues()
    {
        if (Guesses.Any(guessPeg => !_game.FieldValues[FieldCategories.Colors].Contains(guessPeg.ToString())))
            throw new ArgumentException("The guess contains an invalid value") { HResult = 4400 };
    }

    protected override SimpleColorResult GetCoreResult()
    {
        // Check black and white keyPegs
        List<string> codesToCheck = [.. _game.Codes.ToPegs<ColorField>().Select(cf => cf.ToString()) ];
        List<string> guessPegsToCheck = [.. Guesses.Select(g => g.ToString())];
        List<int> positionsToIgnore = []; 

        ResultValue[] results = [.. Enumerable.Repeat(ResultValue.Incorrect, 4)];

        // check black
        for (int i = 0; i < guessPegsToCheck.Count; i++)
        {
            // check black
            if (guessPegsToCheck[i] == codesToCheck[i])
            {
                results[i] = ResultValue.CorrectPositionAndColor;
                positionsToIgnore.Add(i);
                codesToCheck[i] = string.Empty;
            }
        }

        // check white
        for (int i = 0; i < guessPegsToCheck.Count; i++)
        {
            if (positionsToIgnore.Contains(i)) continue;
            int ix = codesToCheck.IndexOf(guessPegsToCheck[i]);
            if (ix == -1) continue;
            results[i] = ResultValue.CorrectColor;
            codesToCheck[ix] = string.Empty;
        }
 
        return new SimpleColorResult(results);
    }

    protected override void SetGameEndInformation(SimpleColorResult result)
    {
        bool allCorrect = result.Results.All(r => r == ResultValue.CorrectPositionAndColor);

        if (allCorrect || _game.LastMoveNumber >= _game.MaxMoves)
        {
            _game.EndTime = DateTime.UtcNow;
            _game.Duration = _game.EndTime - _game.StartTime;
        }
        if (allCorrect)
        {
            _game.IsVictory = true;
        }
    }
}
