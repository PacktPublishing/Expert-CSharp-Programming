// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using JsonSerialization.Models;

namespace JsonSerialization.Converters;

/// <summary>
/// Custom <see cref="JsonConverter{T}"/> for <see cref="Money"/>.
/// Serializes to/from a compact string format: "29.99 USD".
/// </summary>
public sealed class MoneyJsonConverter : JsonConverter<Money>
{
    public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
    => writer.WriteStringValue(value.ToString());

    public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Expected string token when parsing Money, but got {reader.TokenType}.");

        string raw = reader.GetString()
            ?? throw new JsonException("Expected a non-null string for Money.");

        ReadOnlySpan<char> span = raw.AsSpan();
        int spaceIndex = span.IndexOf(' ');
        if (spaceIndex < 1 || spaceIndex >= span.Length - 1)
            throw new JsonException($"Invalid Money format: '{raw}'. Expected '<amount> <currency>'.");

        if (!decimal.TryParse(span[..spaceIndex], NumberStyles.Number,
                CultureInfo.InvariantCulture, out decimal amount))
            throw new JsonException($"Cannot parse amount from '{raw}'.");

        string currency = span[(spaceIndex + 1)..].ToString();
        return new Money(amount, currency);
    }
}
