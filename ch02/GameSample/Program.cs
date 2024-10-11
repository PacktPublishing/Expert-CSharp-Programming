IGame<string, (int, int)> game1 = new Game6x4();
game1.StartGame();
var result = game1.SetMove(["Red", "Green", "Blue", "Yellow"]);
Console.WriteLine($"{result.Item1}, {result.Item2}");

IGame<string, (int, int)> game2 = new Game8x5();
game2.StartGame();
var result2 = game2.SetMove(["Black", "Red", "Blue", "Green", "Yellow"]);
Console.WriteLine($"{result2.Item1}, {result2.Item2}");

IGame<(string, string), (int, int, int)> game3 = new Game5x5x4();
game3.StartGame();
var result3 = game3.SetMove([("Red", "Circle"), ("Blue", "Square"), ("Green", "Triangle"), ("Yellow", "Hexagon")]);
Console.WriteLine($"{result3.Item1}, {result3.Item2}, {result3.Item3}");
