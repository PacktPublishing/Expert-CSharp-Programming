namespace Codebreaker.GameAPIs.Analyzers;

public class ShapeGameGuessAnalyzer(IGame game, ShapeAndColorField[] guesses, int moveNumber) : GameGuessAnalyzer<ShapeAndColorField, ShapeAndColorResult>(game, guesses, moveNumber)
{
    protected override void ValidateGuessValues()
    {
        // check for valid colors
        if (!Guesses.Select(f => f.Color.ToString())
            .Any(color => _game.FieldValues[FieldCategories.Colors]
            .Contains(color)))
            throw new ArgumentException("The guess contains an invalid color") { HResult = 4402 };

        // check for valid shapes
        if (!Guesses.Select(f => f.Shape.ToString())
            .Any(shape => _game.FieldValues[FieldCategories.Shapes]
            .Contains(shape)))
            throw new ArgumentException("The guess contains an invalid shape") { HResult = 4403 };
    }

    protected override ShapeAndColorResult GetCoreResult()
    {
        // Check black, white and blue keyPegs
        List<ShapeAndColorField> codesToCheck = [.._game.Codes.ToPegs<ShapeAndColorField>() ]; // all the codes that need to be verified with the actual check
        List<ShapeAndColorField> guessPegsToCheck = [.. Guesses ]; // all the guesses that need to be verified with the actual check
        List<ShapeAndColorField> remainingCodesToCheck = []; // the codes that need to be checked with the check following - filled by the actual check
        List<ShapeAndColorField> remainingGuessPegsToCheck = []; // the guesses that need to be checked with the check following - filled by the actual check

        byte black = 0;
        byte white = 0;
        byte blue = 0;

        // first check for black (correct color and shape at the correct position)
        // add the remaining codes and guess pegs to the remaining lists to check for white and blue keyPegs
        for (int i = 0; i < guessPegsToCheck.Count; i++)
        {
            if (guessPegsToCheck[i] == codesToCheck[i])
            {
                black++;
            }
            else // the codes and the guess pegs need to be checked again for the blue and white keyPegs
            {
                remainingCodesToCheck.Add(codesToCheck[i]);
                remainingGuessPegsToCheck.Add(guessPegsToCheck[i]);
            }
        }

        // next check for white (correct pair at a wrong position)
        // add the remaining codes and guess pegs to the remaining lists to check for blue keyPegs
        codesToCheck = remainingCodesToCheck;
        guessPegsToCheck = remainingGuessPegsToCheck;
        remainingCodesToCheck = new(codesToCheck);
        remainingGuessPegsToCheck = Enumerable.Repeat(ShapeAndColorField.Empty, guessPegsToCheck.Count).ToList();

        for (int i = 0; i < guessPegsToCheck.Count; i++)
        {
            ShapeAndColorField? codeField = codesToCheck.FirstOrDefault(c => c == guessPegsToCheck[i]);
            if (codeField is not null)
            {
                white++;

                var ix = codesToCheck.IndexOf(codeField);
                codesToCheck[ix] = ShapeAndColorField.Empty;  // this code is a match and thus no longer is used when checking for white
                remainingCodesToCheck[ix] = ShapeAndColorField.Empty; // this code is also not used with the next blue check
            }
            else
            {
                remainingGuessPegsToCheck[i] = guessPegsToCheck[i];  // not a match for the guess, thus it needs to be added for the blue check
            }
        }

        // check blue (either the shape or the color is in the correct position but with a wrong paired element)
        codesToCheck = remainingCodesToCheck;
        guessPegsToCheck = remainingGuessPegsToCheck;

        for (int i = 0; i < guessPegsToCheck.Count; i++)
        {
            if ((guessPegsToCheck[i] != ShapeAndColorField.Empty || 
                codesToCheck[i] != ShapeAndColorField.Empty) &&
                (guessPegsToCheck[i].Shape == codesToCheck[i].Shape || 
                guessPegsToCheck[i].Color == codesToCheck[i].Color))
            {
                blue++;
            }
        }

        ShapeAndColorResult resultPegs = new(black, white, blue);

        if ((resultPegs.Correct + resultPegs.WrongPosition + resultPegs.ColorOrShape) > _game.NumberCodes)
            throw new InvalidOperationException("There are more keyPegs than codes"); // Should not be the case

        return resultPegs;
    }

    protected override void SetGameEndInformation(ShapeAndColorResult result)
    {
        bool allCorrect = result.Correct == _game.NumberCodes;
        if (allCorrect || _game.LastMoveNumber >= _game.MaxMoves)
        {
            _game.EndTime = DateTime.UtcNow;
            _game.Duration = _game.EndTime - _game.StartTime;
        }
        _game.IsVictory = allCorrect;
    }
}
