﻿using GamesSample.Models;

namespace GamesSample;

public class GameManager
{
    private static readonly string[] s_colors5 = ["Red", "Blue", "Green", "Yellow", "Purple"];
    private static readonly string[] s_shapes5 = ["Circle", "Square", "Triangle", "Hexagon", "Star"];
    private static readonly string[] s_colors6 = ["Red", "Blue", "Green", "Yellow", "Purple", "Orange"];
    private static readonly string[] s_colors8 = ["Red", "Blue", "Green", "Yellow", "Purple", "Orange", "Pink", "Brown"];

    private Dictionary<Guid, IGame> _runningGames = new();

    private static string[] GenerateColorSolution(string[] colors, int numberFields) =>
    [.. Random.Shared.GetItems(colors, numberFields)];

    private static (string Color, string Shape)[] GenerateShapeSolution(string[] colors, string[] shapes, int numberFields)
    {
        var colorSolution = Random.Shared.GetItems(colors, numberFields);
        var shapeSolution = Random.Shared.GetItems(shapes, numberFields);
        return [.. colorSolution.Zip(shapeSolution).Select(x => (x.First, x.Second))];
    }

    public TGame StartGame<TGame>(string gameType, string playerName)
        where TGame : IGame
    {
        static ColorGame CreateColorGame(string gameType, string playerName, int numberFields, string[] solution)
        {
            return new ColorGame(gameType, playerName, numberFields, solution);
        }

        ShapeGame CreateShapeGame(string gameType, string playerName, int numberFields, (string Color, string Shape)[] solution)
        {
            return new ShapeGame(gameType, playerName, numberFields, solution);
        }

        TGame game = gameType switch
        {
            "Game6x4" => (TGame)(IGame)CreateColorGame(gameType, playerName, 4, GenerateColorSolution(s_colors6, 4)),
            "Game8x5" => (TGame)(IGame)CreateColorGame(gameType, playerName, 5, GenerateColorSolution(s_colors8, 5)),
            "Game5x5x4" => (TGame)(IGame)CreateShapeGame(gameType, playerName, 4, GenerateShapeSolution(s_colors5, s_shapes5, 4)),
            _ => throw new ArgumentException("Game type not available")
        };
        _runningGames.Add(game.GameId, game);
        return game;
    }

    public TGame StartGame2<TGame>(string gameType, string playerName)
    where TGame : IGame
    {
        static ColorGame CreateColorGame(string gameType, string playerName, int numberFields, string[] solution)
        {
            return new ColorGame(gameType, playerName, numberFields, solution);
        }

        ShapeGame CreateShapeGame(string gameType, string playerName, int numberFields, (string Color, string Shape)[] solution)
        {
            return new ShapeGame(gameType, playerName, numberFields, solution);
        }

        TGame game = gameType switch
        {
            "Game6x4" => (TGame)(IGame)CreateColorGame(gameType, playerName, 4, GenerateColorSolution(s_colors6, 4)),
            "Game8x5" => (TGame)(IGame)CreateColorGame(gameType, playerName, 5, GenerateColorSolution(s_colors8, 5)),
            "Game5x5x4" => (TGame)(IGame)CreateShapeGame(gameType, playerName, 4, GenerateShapeSolution(s_colors5, s_shapes5, 4)),
            _ => throw new ArgumentException("Game type not available")
        };
        _runningGames.Add(game.GameId, game);
        return game;
    }

    public TResult SetMove<TField, TResult>(Guid gameId, TField[] guesses)
        where TField : struct
        where TResult : struct
    {
        if (!_runningGames.TryGetValue(gameId, out IGame? game))
        {
            throw new InvalidOperationException("game not found");
        }
        if (game is not Game<TField, TResult> gameT)
        {
            throw new InvalidOperationException("game type mismatch");
        }
        TResult result = gameT.AddMove(guesses);
        return result;
    }
}
