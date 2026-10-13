// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace WpfMvvmToolkit;

public sealed class AppUiOptions
{
    [Required]
    public string WindowTitle { get; init; } = "WPF MVVM Toolkit Startup";

    [Required]
    public string WelcomeName { get; init; } = "WPF User";
}
