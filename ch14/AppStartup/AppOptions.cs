// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace AppStartup;

/// <summary>
/// Strongly-typed representation of the "AppOptions" configuration section.
/// DataAnnotations on properties are validated automatically by AddOptions().
/// </summary>
public sealed class AppOptions
{
    [System.ComponentModel.DataAnnotations.Required]
    public string Name { get; init; } = "AppStartupSample";
    public string Version { get; init; } = "1.0.0";

    [System.ComponentModel.DataAnnotations.Range(1, 1000)]
    public int MaxItems { get; init; } = 100;
}
