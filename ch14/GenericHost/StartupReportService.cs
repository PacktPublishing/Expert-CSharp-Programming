// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHost;

/// <summary>
/// Demonstrates IHostedService for one-shot work at startup and teardown.
/// StartAsync is called once when the host starts, StopAsync once on shutdown.
/// Use this pattern for: cache warming, DB migration checks, connection tests.
/// </summary>
public sealed class StartupReportService(
    ILogger<StartupReportService> logger,
    IHostEnvironment env) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // This runs synchronously before the host signals ApplicationStarted.
        logger.LogInformation(
            "Application starting | Environment: {Env} | App: {App} | Root: {Root}",
            env.EnvironmentName, env.ApplicationName, env.ContentRootPath);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartupReportService teardown complete");
        return Task.CompletedTask;
    }
}
