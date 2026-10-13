// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WpfMvvmToolkit.Services;
using WpfMvvmToolkit.ViewModels;

namespace WpfMvvmToolkit;

public partial class App : Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = Host.CreateApplicationBuilder(e.Args);

        builder.Services.AddOptions<AppUiOptions>()
            .BindConfiguration("AppUi")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton<ITimeProvider, UtcTimeProvider>();
        builder.Services.AddSingleton<MainWindowViewModel>();
        builder.Services.AddSingleton<MainWindow>();

        _host = builder.Build();

        await _host.StartAsync();

        var logger = _host.Services.GetRequiredService<ILogger<App>>();
        var uiOptions = _host.Services.GetRequiredService<IOptions<AppUiOptions>>().Value;

        logger.LogInformation("WPF host started | Window title: {Title}", uiOptions.WindowTitle);

        var window = _host.Services.GetRequiredService<MainWindow>();
        window.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
