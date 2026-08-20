// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace JsonSerialization.Models;

/// <summary>Represents a single book in a library catalog.</summary>
public record class Book(
    int Id,
    string Title,
    string Author,
    Money Price,
    DateOnly PublishedOn,
    IReadOnlyList<string> Tags)
{
    [JsonIgnore]
    public string DisplayTitle => $"{Title} by {Author}";
}
