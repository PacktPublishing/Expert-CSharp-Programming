// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHost;

// ─────────────────────────────────────────────────────────────────────────
// HeartbeatService — long-running BackgroundService
// ─────────────────────────────────────────────────────────────────────────

/// <summary>
/// Demonstrates BackgroundService — the recommended base class for long-running
/// tasks. ExecuteAsync runs for the lifetime of the host; the stoppingToken is
/// cancelled when the host initiates shutdown, so the loop exits cleanly.
/// </summary>
public sealed class HeartbeatService(
    ILogger<HeartbeatService> logger,
    IMetricsCollector metrics,
    IHostApplicationLifetime lifetime) : BackgroundService
{
    // How many heartbeats to emit before requesting shutdown (demo only —
    // real services typically run until Ctrl+C or a process signal).
    private const int TotalHeartbeats = 5;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("HeartbeatService started — will run {Count} heartbeats", TotalHeartbeats);

        for (int i = 1; i <= TotalHeartbeats; i++)
        {
            // Honour the cancellation token: exit immediately on shutdown.
            if (stoppingToken.IsCancellationRequested)
                break;

            metrics.RecordHeartbeat();

            // Structured log message — each {} placeholder is a named property,
            // making logs queryable in Seq, Elastic, Application Insights, etc.
            logger.LogInformation(
                "Heartbeat {Index}/{Total} | Total recorded: {TotalBeats}",
                i, TotalHeartbeats, metrics.HeartbeatCount);

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }

        logger.LogInformation("HeartbeatService finished all heartbeats — requesting graceful shutdown");

        // StopApplication() signals the host to begin graceful shutdown,
        // which cancels stoppingToken for all other hosted services.
        lifetime.StopApplication();
    }
}
