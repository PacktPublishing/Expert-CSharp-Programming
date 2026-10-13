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

sealed class PaymentService(ILogger<PaymentService> logger)
{
    public void Charge(int orderId, decimal amount)
    {
        logger.LogInformation("PaymentService: charging {Amount:C} for order {OrderId}", amount, orderId);
        // … payment gateway call …
        logger.LogInformation("PaymentService: payment authorised for order {OrderId}", orderId);
    }
}
