// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ─────────────────────────────────────────────────────────────────────────
// BuiltInLoggingDemo — demonstrates ILogger<T> and log levels
// ─────────────────────────────────────────────────────────────────────────
sealed class LoggingDemo(
    ILogger<LoggingDemo> logger,
    OrderService orderSvc,
    PaymentService paymentSvc,
    IHostApplicationLifetime lifetime) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // ── Log levels ─────────────────────────────────────────────────────
        // Levels in ascending severity: Trace → Debug → Information → Warning
        //                               → Error → Critical
        // The active minimum level (from config) determines what is emitted.
        logger.LogTrace("Trace - finest detail, usually disabled in production");
        logger.LogDebug("Debug - development diagnostics");
        logger.LogInformation("Information - normal operational events");
        logger.LogWarning("Warning - unexpected but recoverable situation");
        logger.LogError("Error - operation failed, needs attention");
        logger.LogCritical("Critical - system cannot continue, immediate action needed");

        Console.WriteLine();

        // ── Structured logging — ALWAYS use named placeholders ────────────
        // Named placeholders ({OrderId}, {Amount}) create queryable properties
        // in log aggregators (Seq, Elastic, Application Insights).
        // Avoid string interpolation ($"…") — it loses structure.
        int orderId = 42;
        decimal amount = 199.99m;
        logger.LogInformation("Order {OrderId} created for amount {Amount:C}", orderId, amount);

        // ── Logging with scopes ────────────────────────────────────────────
        // BeginScope attaches extra key/value pairs to every log message
        // emitted inside the using block — useful for correlation IDs.
        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = Guid.NewGuid().ToString("N")[..8],
            ["UserId"] = "user-123",
        }))
        {
            logger.LogInformation("Processing order inside a logging scope");
            orderSvc.ProcessOrder(orderId);
        }

        // ── Other services logging via their own ILogger<T> ───────────────
        paymentSvc.Charge(orderId, amount);

        lifetime.StopApplication();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
