using System.Text.Json.Serialization;

namespace GameModels;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(IEnumerable<Game>))]
[JsonSerializable(typeof(ColorGame))]
[JsonSerializable(typeof(ShapeAndColorGame))]
[JsonSerializable(typeof(Move))]
[JsonSerializable(typeof(ColorMove))]
[JsonSerializable(typeof(ShapeAndColorMove))]
[JsonSerializable(typeof(ShapeAndColor))]
[JsonSerializable(typeof(ShapeAndColor[]))]
[JsonSerializable(typeof(ColorResult))]
[JsonSerializable(typeof(ShapeAndColorResult))]
[JsonSerializable(typeof(IResult))]
[JsonSerializable(typeof(List<Move>))]
public partial class JsonGamesContext : JsonSerializerContext
{
}
