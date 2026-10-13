// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Configuration;

/// <summary>
/// Maps to the "FeatureFlags" section — a typical feature-toggle object.
/// Works with IOptionsMonitor for live reload: edit appsettings.json and the
/// app picks up the new values without a restart.
/// </summary>
public sealed class FeatureFlags
{
    public bool EnableDarkMode { get; init; }
    public bool EnableBetaFeatures { get; init; }

    [Range(1, 500)]
    public int MaxUploadSizeMb { get; init; } = 10;
}
