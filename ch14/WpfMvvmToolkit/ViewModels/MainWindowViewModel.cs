// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WpfMvvmToolkit.Services;

namespace WpfMvvmToolkit.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel(
        ITimeProvider timeProvider,
        IOptions<AppUiOptions> options,
        ILogger<MainWindowViewModel> logger)
    {
        _timeProvider = timeProvider;
        _logger = logger;

        var appOptions = options.Value;

        WindowTitle = appOptions.WindowTitle;
        Greeting = $"Hello, {appOptions.WelcomeName}!";
        LastUpdatedUtc = _timeProvider.UtcNow.ToString("O");
    }

    private readonly ITimeProvider _timeProvider;
    private readonly ILogger<MainWindowViewModel> _logger;

    [ObservableProperty]
    public partial string WindowTitle { get; set; }

    [ObservableProperty]
    public partial string Greeting { get; set; }

    [ObservableProperty]
    public partial string LastUpdatedUtc { get; set; }

    public string StartupNote { get; } = "This sample uses Host.CreateApplicationBuilder in App.xaml.cs to centralize DI, configuration, and logging in a WPF app.";

    [RelayCommand]
    private void Refresh()
    {
        LastUpdatedUtc = _timeProvider.UtcNow.ToString("O");

        _logger.LogInformation(
            "Refresh executed at {UtcTime}",
            LastUpdatedUtc);
    }
}
