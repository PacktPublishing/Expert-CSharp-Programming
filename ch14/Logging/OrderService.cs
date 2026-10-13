// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

/// <summary>
/// ILogger<T> uses the full type name as the log category, so log entries can
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
