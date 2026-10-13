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
// SerilogLoggingDemo — same demo wired through Serilog
// ─────────────────────────────────────────────────────────────────────────
sealed class SerilogLoggingDemo(
    ILogger<SerilogLoggingDemo> logger,
    OrderService orderSvc,
    PaymentService paymentSvc,
    IHostApplicationLifetime lifetime) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Serilog demo started — same ILogger<T> API, different back-end");

        int orderId = 99;
        decimal amount = 349.50m;

        // Structured properties flow into Serilog's JSON/file output unchanged
        logger.LogInformation("Order {OrderId} received for {Amount:C}", orderId, amount);

        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = Guid.NewGuid().ToString("N")[..8],
        }))
        {
            orderSvc.ProcessOrder(orderId);
            paymentSvc.Charge(orderId, amount);
        }

        logger.LogWarning("Demo complete — check logs/ directory for file output");

        lifetime.StopApplication();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

// ─────────────────────────────────────────────────────────────────────────
// AppLogMessages — [LoggerMessage] source generator extension methods
// (defined in AppLogMessages.cs — source generators require a separate file
//  when the main file uses top-level statements)
// ─────────────────────────────────────────────────────────────────────────
