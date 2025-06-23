using System.Text.Json.Serialization;

namespace Ch08.DataLib;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(List<int>))]
internal partial class JsonContext : JsonSerializerContext
{
}