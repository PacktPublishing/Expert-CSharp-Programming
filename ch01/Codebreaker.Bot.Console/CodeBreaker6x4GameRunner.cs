using Codebreaker.GameAPIs.Client;
using Codebreaker.GameAPIs.Client.Models;

using Microsoft.Extensions.Logging;

namespace CodeBreaker.Bot;

public class CodeBreaker6x4GameRunner(GamesClient gamesClient, ILogger<CodeBreaker6x4GameRunner> logger)
{
    private const string PlayerName = "Bot";
    private Guid? _gameId;
    private int _moveNumber = 0;
    private readonly List<Move> _moves = [];
    private List<string[]>? _possibleValues;
    private readonly List<string> _colorNames = [];
    private readonly ILogger _logger = logger;
    private readonly GamesClient _gamesClient = gamesClient;

    // initialize a list of all the possible options using numbers for every color
    private List<string[]> InitializePossibleValues()
    {
        if (_colorNames is null || _colorNames.Count != 6)
        {
            return [];
        }
        List<string[]> possibleValues = [];

        int colorCount = _colorNames?.Count ?? 0;
        if (colorCount == 0)
        {
            return possibleValues;
        }

        for (int i = 0; i < colorCount; i++)
        {
            for (int j = 0; j < colorCount; j++)
            {
                for (int k = 0; k < colorCount; k++)
                {
                    for (int l = 0; l < colorCount; l++)
                    {
                        string[] colorCombination = [_colorNames![i], _colorNames[j], _colorNames[k], _colorNames[l]];
                        possibleValues.Add(colorCombination);
                    }
                }
            }
        }
        return possibleValues;
    }

    public async Task StartGameAsync(CancellationToken cancellationToken = default)
    {
        static int NextKey(ref int key)
        {
            int next = key;
            key <<= 1;
            return next;
        }

        // start the game, get the game id and the possible color values
        (_gameId, _, _, IDictionary<string, string[]> fieldValues) = await _gamesClient.StartGameAsync(GameType.Game6x4, "Bot", cancellationToken);
        int key = 1;
        IDictionary<int, string> colorNames = fieldValues["colors"]
            .ToDictionary(keySelector: c => NextKey(ref key), elementSelector: color => color);
        _colorNames.Clear();
        _colorNames.AddRange(colorNames.Values);

        _possibleValues = InitializePossibleValues();
        _moves.Clear();
        _moveNumber = 0;
    }

    /// <summary>
    /// Sends moves to the API server
    /// Moves are delayed by thinkSeconds before setting the next move
    /// Finishes when the game is over
    /// </summary>
    /// <param name="thinkSeconds">The seconds to simulate thinking before setting the next move</param>
    /// <returns>a task</returns>
    /// <exception cref="InvalidOperationException">throws if initialization was not done, or with invalid game state</exception>
    public async Task RunAsync(int thinkSeconds, CancellationToken cancellationToken = default)
    {
        if (_possibleValues is null) 
            throw new InvalidOperationException($"call {nameof(StartGameAsync)} before");
        Guid gameId = _gameId ?? 
            throw new InvalidOperationException($"call {nameof(StartGameAsync)} before");

        bool hasEnded = false;
        do
        {
            _moveNumber++;
            var guessPegs = GetNextMoves();
            _logger.SendMove(string.Join(':', guessPegs), gameId.ToString());

            (string[] results, hasEnded, bool isVictory) = await _gamesClient.SetMoveAsync(gameId, PlayerName, GameType.Game6x4, _moveNumber, [.. guessPegs ], cancellationToken);
            string resultValue = results.Length == 0 ? "none" : string.Join(':', results);
            _logger.ReceivedResult(resultValue, _moveNumber, gameId.ToString());

            if (isVictory)
            {
                _logger.Matched(_moveNumber, gameId.ToString());
                break;
            }
            if (hasEnded)
            {
                _logger.Ended(_moveNumber, gameId.ToString());
                break;
            }

            int blackHits = results.Count(c => c == "Black");
            int whiteHits = results.Count(c => c == "White");

            if (blackHits >= 4)
                throw new InvalidOperationException($"4 or more blacks but won was not set: {blackHits}");

            if (whiteHits > 4)
                throw new InvalidOperationException($"more than 4 whites is not possible: {whiteHits}");

            if (blackHits == 0 && whiteHits == 0)
            {
                _possibleValues = _possibleValues.ProcessNoMatches(guessPegs);
                _logger.ReducedPossibleValues(_possibleValues.Count, "none", gameId.ToString());
            }
            if (blackHits > 0)
            {
                _possibleValues = _possibleValues.ProcessBlackMatches(blackHits, guessPegs);
                _logger.ReducedPossibleValues(_possibleValues.Count, "Black", gameId.ToString());
            }
            if (whiteHits > 0)
            {
                _possibleValues = _possibleValues.ProcessWhiteMatches(whiteHits + blackHits, guessPegs);
                _logger.ReducedPossibleValues(_possibleValues.Count, "White", gameId.ToString());
            }

            await Task.Delay(TimeSpan.FromSeconds(thinkSeconds), cancellationToken);  // thinking delay
        } while (!hasEnded);

        _logger.FinishedGame(_moveNumber, gameId.ToString());
    }

    /// <summary>
    /// Get the values for the next move
    /// </summary>
    /// <returns>A string and int representation for the next moves</returns>
    /// <exception cref="InvalidOperationException">Throws if there are no calculated possible values left to chose from</exception>
    private string[] GetNextMoves()
    {
        if (_possibleValues?.Count is null or 0) 
            throw new InvalidOperationException("invalid number of possible values - 0");

        int random = Random.Shared.Next(_possibleValues.Count);
        return _possibleValues[random];
    }
}

public enum KeyColors
{
    Black,
    White
}

public record struct CodePeg(string Color)
{
    public override readonly string ToString() => Color;
}

public record struct KeyPeg(string Color)
{
    public override readonly string ToString() => Color;
}

public record struct Move(string GameId, int MoveNumber, IList<CodePeg> Codes, IList<KeyPeg> Keys);
