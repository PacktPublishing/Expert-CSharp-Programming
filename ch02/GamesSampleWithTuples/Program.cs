using GamesSample;
using GamesSample.Models;

GameManager manager = new();

var game1 = manager.StartGame<ColorGame>("Game6x4", "TestPlayer");
(int correctPosition, int incorrectPosition) = game1.AddMove(["Red", "Green", "Blue", "Yellow"]);
Console.WriteLine($"{correctPosition}, {incorrectPosition}");

var game2 = manager.StartGame<ColorGame>("Game8x5", "TestPlayer");
(correctPosition, incorrectPosition) = game2.AddMove(["Black", "Red", "Blue", "Green", "Yellow"]);
Console.WriteLine($"{correctPosition}, {incorrectPosition}");

var game3 = manager.StartGame<ShapeGame>("Game5x5x4", "TestPlayer");
(correctPosition, incorrectPosition, int partialCorrect) = game3.AddMove([new ("Red", "Circle"), new ("Blue", "Square"), new ("Green", "Triangle"), new ("Yellow", "Hexagon")]);
Console.WriteLine($"{correctPosition}, {incorrectPosition}, {partialCorrect}");
