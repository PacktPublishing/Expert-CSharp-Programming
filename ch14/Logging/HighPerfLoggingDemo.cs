// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

//
// Demonstrates:
//   1. Built-in Microsoft.Extensions.Logging providers (Console, Debug)
//   2. Log levels and category-based filtering via appsettings.json
//   3. Structured logging — named message-template placeholders
//   4. Serilog as a drop-in replacement with output templates and file sink
//   5. High-performance logging with [LoggerMessage] source generator (Part C)
//   6. ILogger<T> injection via DI

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ─────────────────────────────────────────────────────────────────────────
// HighPerfLoggingDemo — [LoggerMessage] source generator (Part C)
// ─────────────────────────────────────────────────────────────────────────

/// <summary>
/// Demonstrates the [LoggerMessage] source generator — the preferred approach
/// for hot-path logging in production code because it:
///   • Avoids boxing of value-type parameters (no params object[] allocation)
///   • Checks the enabled log level before formatting the message string
///   • Produces a compile-time generated, allocation-free log delegate
///
/// The static partial methods are defined as extension methods on ILogger in
/// <see cref="AppLogMessages"/> so they can be called directly on any ILogger.
/// </summary>
sealed class HighPerfLoggingDemo(
    ILogger<HighPerfLoggingDemo> logger,
    IHostApplicationLifetime lifetime) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine("📌 [LoggerMessage] extension-method pattern:");
        Console.WriteLine("   Each method is source-generated — no boxing, no string allocation if level is disabled.");
        Console.WriteLine();

        int orderId = 101;
        decimal amount = 75.00m;

        // Call the generated extension methods directly on ILogger<T>.
        // These compile to the same fast path as hand-rolled static delegates.
        logger.OrderProcessing(orderId);
        logger.OrderProcessed(orderId);
        logger.PaymentCharging(amount, orderId);
        logger.PaymentAuthorised(orderId);

        logger.LogInformation("HighPerfLoggingDemo complete — all calls used [LoggerMessage] methods");

        lifetime.StopApplication();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
