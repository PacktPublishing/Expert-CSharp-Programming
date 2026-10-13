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

using Microsoft.Extensions.Logging;
// ─────────────────────────────────────────────────────────────────────────
// Application services — each owns its own ILogger<T>
// ─────────────────────────────────────────────────────────────────────────

/// <summary>
/// ILogger&lt;T&gt; uses the full type name as the log category, so log entries can
/// be filtered per-service in appsettings.json or programmatically.
/// </summary>
sealed class OrderService(ILogger<OrderService> logger)
{
    public void ProcessOrder(int orderId)
    {
        logger.LogDebug("OrderService: processing order {OrderId}", orderId);
        // … order processing logic …
        logger.LogInformation("OrderService: order {OrderId} processed successfully", orderId);
    }
}
