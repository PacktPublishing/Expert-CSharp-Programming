// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

/// <summary>
/// Static partial class holding all compile-time log definitions for this app.
///
/// Rules for [LoggerMessage] source generation:
///   • The class must be <c>static</c> and <c>partial</c>.
///   • Each method must be <c>static partial</c> and return <c>void</c>.
///   • Decorate with <c>[LoggerMessage(EventId, Level, Message)]</c>.
///   • The first parameter is <c>this ILogger logger</c> — making each method
///     a callable extension method on any <c>ILogger</c> or <c>ILogger&lt;T&gt;</c>.
///   • Use named placeholders in <c>Message</c> that match parameter names exactly.
///
/// Benefits over <c>logger.LogInformation("…", args)</c>:
///   • No boxing of value-type parameters (no <c>params object[]</c> allocation)
///   • Log-level check happens before any string formatting
///   • The compiler generates a cached delegate — zero overhead when disabled
/// </summary>
internal static partial class AppLogMessages
{
    [LoggerMessage(EventId = 100, Level = LogLevel.Debug,
        Message = "Processing order {OrderId}")]
    public static partial void OrderProcessing(this ILogger logger, int orderId);

    [LoggerMessage(EventId = 101, Level = LogLevel.Information,
        Message = "Order {OrderId} processed successfully")]
    public static partial void OrderProcessed(this ILogger logger, int orderId);

    [LoggerMessage(EventId = 200, Level = LogLevel.Information,
        Message = "Charging {Amount:C} for order {OrderId}")]
    public static partial void PaymentCharging(this ILogger logger, decimal amount, int orderId);

    [LoggerMessage(EventId = 201, Level = LogLevel.Information,
        Message = "Payment authorised for order {OrderId}")]
    public static partial void PaymentAuthorised(this ILogger logger, int orderId);
}
