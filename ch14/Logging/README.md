# Logging — Providers, Log Levels & Structured Logging

This sample demonstrates **.NET logging** from three angles:
1. **Built-in `Microsoft.Extensions.Logging`** — Console + Debug providers, log levels, structured messages, and scopes
2. **Serilog** — a popular third-party back-end with console and rolling-file sinks
3. **`[LoggerMessage]` source generator** — allocation-free, high-performance compile-time log definitions

## 🚀 Features Demonstrated

### 1. Log Levels

| Level | Method | When to use |
|---|---|---|
| `Trace` | `LogTrace()` | Finest detail — almost always disabled in production |
| `Debug` | `LogDebug()` | Development diagnostics |
| `Information` | `LogInformation()` | Normal operational events |
| `Warning` | `LogWarning()` | Unexpected but recoverable |
| `Error` | `LogError()` | Operation failed, needs attention |
| `Critical` | `LogCritical()` | System cannot continue |

### 2. Category-Based Filtering
Filter log output per namespace or type in `appsettings.json`:
```json
"Logging": {
  "LogLevel": {
    "Default": "Debug",
    "Microsoft": "Warning",
    "OrderService": "Debug"
  }
}
```

### 3. Structured Logging — Named Placeholders
```csharp
// ✅ Do this — named placeholders create queryable properties
logger.LogInformation("Order {OrderId} created for {Amount:C}", orderId, amount);

// ❌ Not this — string interpolation destroys the structure
logger.LogInformation($"Order {orderId} created for {amount:C}");
```

### 4. Logging Scopes — Correlation IDs
```csharp
using (logger.BeginScope(new Dictionary<string, object>
{
    ["CorrelationId"] = correlationId,
    ["UserId"] = userId,
}))
{
    // Every log message inside this block carries CorrelationId and UserId
    logger.LogInformation("Processing request");
}
```

### 5. Serilog Integration
```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}")
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Replace built-in providers with Serilog:
builder.Services.AddSerilog();
```

### 6. ILogger\<T\> via DI
```csharp
sealed class OrderService(ILogger<OrderService> logger)
{
    public void ProcessOrder(int id)
    {
        logger.LogDebug("Processing order {OrderId}", id);
        // Type name becomes the log category — filterable per service
    }
}
```

### 7. `[LoggerMessage]` Source Generator — High-Performance Logging
The C# source generator creates allocation-free, level-checked log delegates at compile time.
Define log messages as `static partial` extension methods on `ILogger`:
```csharp
// Define once in a static partial class
static partial class AppLogMessages
{
    [LoggerMessage(EventId = 100, Level = LogLevel.Debug,
        Message = "Processing order {OrderId}")]
    public static partial void OrderProcessing(this ILogger logger, int orderId);

    [LoggerMessage(EventId = 200, Level = LogLevel.Information,
        Message = "Charging {Amount:C} for order {OrderId}")]
    public static partial void PaymentCharging(this ILogger logger, decimal amount, int orderId);
}

// Call like a normal method — no boxing, no string allocation when level disabled
logger.OrderProcessing(orderId);
logger.PaymentCharging(amount, orderId);
```

## 📋 Prerequisites
- **.NET 10 SDK** or later

## 🔧 Run

```bash
cd ch14/Logging
dotnet run
# Serilog file output appears in ./logs/app-<date>.log
```

## 📊 Sample Output

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
PART A — Built-in Microsoft.Extensions.Logging
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
HH:mm:ss trce BuiltInLoggingDemo 🔍 Trace  — finest detail, usually disabled in production
HH:mm:ss dbug BuiltInLoggingDemo 🐛 Debug  — development diagnostics
HH:mm:ss info BuiltInLoggingDemo ℹ️  Information — normal operational events
…

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
PART B — Serilog (structured logging + file sink)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
[HH:mm:ss INF] [SerilogLoggingDemo] Serilog demo started…
[HH:mm:ss INF] [SerilogLoggingDemo] Order 99 received for $349.50
…
✅ Logging demo complete. Check ./logs/ for the Serilog file output.
```
