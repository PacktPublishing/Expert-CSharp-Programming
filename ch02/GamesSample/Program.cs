using GamesSample;
using GamesSample.Models;

GameManager manager = new();

var game1 = manager.StartGame<ColorGame>("Game6x4", "TestPlayer");
string[] guesses = ["Red", "Green", "Blue", "Yellow"];
(int correctPosition, int incorrectPosition) = game1.AddMove(guesses);
Console.WriteLine($"Game6x4 - the move {string.Join(", ", guesses)} results in {correctPosition} correct positions and {incorrectPosition} incorrect positions");

var game2 = manager.StartGame<ColorGame>("Game8x5", "TestPlayer");
guesses = ["Red", "Blue", "Green", "Yellow", "Purple"];
(correctPosition, incorrectPosition) = game2.AddMove(guesses);
Console.WriteLine($"Game8x5 - the move {string.Join(", ", guesses)} results in {correctPosition} correct positions and {incorrectPosition} incorrect positions");

#if USERECORDS

var game3 = manager.StartGame<ShapeGame>("Game5x5x4", "TestPlayer");

ShapeField[] colorAndShapeGuesses = [
    new ("Red", "Circle"),
    new ("Blue", "Square"),
    new ("Green", "Triangle"),
    new ("Yellow", "Hexagon")
];
(correctPosition, incorrectPosition, int partialCorrect) = game3.AddMove(colorAndShapeGuesses);
string colorAndShapeGuessesOutput = string.Join(", ", colorAndShapeGuesses.Select(x => $"{x.Color} {x.Shape}"));
Console.WriteLine($"Game5x5x4 - Move {colorAndShapeGuessesOutput}");
Console.WriteLine($"Result: correct position for pair: {correctPosition}, incorrect position for pair: {incorrectPosition}, color or shape correct: {partialCorrect}");

#else

var game3 = manager.StartGame<ShapeGame>("Game5x5x4", "TestPlayer");

(string Color, string Shape)[] colorAndShapeGuesses = [
    ("Red", "Circle"),
    ("Blue", "Square"),
    ("Green", "Triangle"),
    ("Yellow", "Hexagon")
];

(correctPosition, incorrectPosition, int partialCorrect) = game3.AddMove(colorAndShapeGuesses);
string colorAndShapeGuessesOutput = string.Join(", ", colorAndShapeGuesses.Select(x => $"{x.Color} {x.Shape}"));
Console.WriteLine($"Game5x5x4 - Move {colorAndShapeGuessesOutput}");
Console.WriteLine($"Result: correct position for pair: {correctPosition}, incorrect position for pair: {incorrectPosition}, color or shape correct: {partialCorrect}");


#endif