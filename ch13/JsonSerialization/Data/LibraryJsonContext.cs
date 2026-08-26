// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

using JsonSerialization.Models;

namespace JsonSerialization.Data;

/// <summary>
/// AOT-safe JSON serializer context generated at compile time.
/// Avoids runtime reflection and is required for Native AOT / Trimming.
/// </summary>
[JsonSerializable(typeof(Book))]
[JsonSerializable(typeof(Library))]
[JsonSerializable(typeof(List<Book>))]
[JsonSerializable(typeof(PagedResult<Book>))]
[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Converters = [typeof(Converters.MoneyJsonConverter)])]
public partial class LibraryJsonContext : JsonSerializerContext { }
