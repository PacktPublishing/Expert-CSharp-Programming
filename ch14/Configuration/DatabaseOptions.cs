// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Configuration;

/// <summary>Maps to the "Database" section in appsettings.json.</summary>
public sealed class DatabaseOptions
{
    [Required]
    public string ConnectionString { get; init; } = string.Empty;

    [Range(1, 300)]
    public int CommandTimeoutSeconds { get; init; } = 30;

    public bool EnableRetry { get; init; } = true;

    [Range(0, 10)]
    public int MaxRetryCount { get; init; } = 3;
}
