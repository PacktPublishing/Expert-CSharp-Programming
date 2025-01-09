using GameModels;

using System.Text.Json;

List<Game> games = [];

ColorGame game1 = new("player1") 
{ 
    Solution = ["red", "green", "blue", "yellow"] 
};
ColorMove move1 = new(1)
{
    Colors = ["red", "green", "blue", "yellow"]
};
ColorResult result1 = game1.SetMove(move1);

games.Add(game1);

ShapeAndColorGame game2 = new("player2") 
{ 
    Solution = [("circle", "red"), ("square", "green"), ("triangle", "blue"), ("star", "yellow")] 
};
ShapeAndColorMove move2 = new(1)
{
    ShapesAndColors = [("circle", "red"), ("triangle", "blue"), ("triangle", "green"), ("star", "blue")]
};
ShapeAndColorResult result2 = game2.SetMove(move2);

games.Add(game2);

string json = JsonSerializer.Serialize(games, typeof(IEnumerable<Game>), JsonGamesContext.Default);
Console.WriteLine(json);

object? gamesDeserialized = JsonSerializer.Deserialize(json, typeof(IEnumerable<Game>), JsonGamesContext.Default);

if (gamesDeserialized is IEnumerable<Game> games2)
{
    foreach (Game game in games2)
    {
        Console.WriteLine($"Player: {game.PlayerName}");
        Console.WriteLine($"Start time: {game.StartTime}");
        Console.WriteLine($"End time: {game.EndTime}");
        Console.WriteLine($"Is won: {game.IsWon}");
        Console.WriteLine($"Last move number: {game.LastMoveNumber}");
        Console.WriteLine($"Moves:");
        foreach (Move move in game.Moves)
        {
            Console.WriteLine($"Move number: {move.MoveNumber}");
            Console.WriteLine($"Move time: {move.MoveTime}");
            if (move is ColorMove colorMove)
            {
                Console.WriteLine($"Colors: {string.Join(", ", colorMove.Colors)}");
            }
            else if (move is ShapeAndColorMove shapeAndColorMove)
            {
                Console.WriteLine($"Shapes and colors: {string.Join(", ", shapeAndColorMove.ShapesAndColors.AsEnumerable())}");
            }
        }
    }
}
else
{
    Console.WriteLine("Deserialization failed");
}

