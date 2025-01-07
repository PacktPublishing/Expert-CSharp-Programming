using GameModels;

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

List<Game> games = new();

ColorGame game1 = new ColorGame("player1", ["red", "green", "blue", "yellow"]);
ColorMove move1 = new ColorMove(game1, 1, ["red", "green", "blue", "yellow"]);
ColorResult result1 = game1.SetMove(move1);

games.Add(game1);


ShapeAndColorGame game2 = new("player2", [("circle", "red"), ("square", "green"), ("triangle", "blue"), ("star", "yellow")]);
ShapeAndColorMove move2 = new(game2, 1, [("circle", "red"), ("triangle", "blue"), ("triangle", "green"), ("star", "blue")]);
ShapeAndColorResult result2 = game2.SetMove(move2);

games.Add(game2);

//JsonSerializerOptions options = new()
//{
//    WriteIndented = true,
//    TypeInfoResolver =
//        new DefaultJsonTypeInfoResolver
//        {
//            Modifiers = { PolymorphicTypeInfoResolver }
//        }
//};

//void PolymorphicTypeInfoResolver(JsonTypeInfo typeInfo)
//{
//    if (typeInfo.Type == typeof(Move))
//    {
//        typeInfo.PolymorphismOptions = new()
//        {
//            TypeDiscriminatorPropertyName = "$discriminator",
//            IgnoreUnrecognizedTypeDiscriminators = true,
//            DerivedTypes =
//            {
//                new JsonDerivedType(typeof(ColorMove), "ColorMove"),
//                new JsonDerivedType(typeof(ShapeAndColorMove), "ShapeAndColorMove")
//            }
//        };
//    }
//}

string json = JsonSerializer.Serialize(games, typeof(IEnumerable<Game>), JsonGamesContext.Default);
Console.WriteLine(json);

object? gamesDeserialized = JsonSerializer.Deserialize(json, typeof(IEnumerable<Game>), JsonGamesContext.Default);
//List<Game>? gamesDeserialized = JsonSerializer.Deserialize<List<Game>>(json, options);
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
                Console.WriteLine($"Shapes and colors: {string.Join(", ", shapeAndColorMove.ShapesAndColors)}");
            }
        }
    }
}
else
{
    Console.WriteLine("Deserialization failed");
}

