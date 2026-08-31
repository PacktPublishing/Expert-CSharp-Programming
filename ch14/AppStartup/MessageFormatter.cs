// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Text.Json;

namespace AppStartup;

public record class Customer(int Id, string Name);

public interface IMessageFormatter<T>
{
    string Format(T value);
}

public sealed class JsonMessageFormatter<T> : IMessageFormatter<T>
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public string Format(T value) => JsonSerializer.Serialize(value, Options);
}
