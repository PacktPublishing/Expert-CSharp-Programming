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

using System.Globalization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ─────────────────────────────────────────────────────────────────────────
// PART A — Built-in Microsoft.Extensions.Logging
// ─────────────────────────────────────────────────────────────────────────
// Host.CreateApplicationBuilder() already registers Console + Debug providers
// and applies log-level filters from appsettings.json.
// We ClearProviders() here to control the output precisely for the demo.
var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();     // remove defaults so we can customise
builder.Logging.AddConsole();         // console logger (formatter configured via appsettings.json)
builder.Logging.AddDebug();           // visible in the IDE debugger output window

// Category-level filters: override defaults per namespace or type name
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // reduce framework noise
builder.Logging.AddFilter("OrderService", LogLevel.Debug);   // verbose for our code
builder.Logging.AddFilter("PaymentService", LogLevel.Information);

builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddHostedService<BuiltInLoggingDemo>();

Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
Console.WriteLine("PART A — Built-in Microsoft.Extensions.Logging");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

var msHost = builder.Build();
await msHost.RunAsync();

// ─────────────────────────────────────────────────────────────────────────
// PART B — Serilog as the logging back-end
// ─────────────────────────────────────────────────────────────────────────
Console.WriteLine();
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
Console.WriteLine("PART B — Serilog (structured logging + file sink)");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

// Configure Serilog globally before building the host so that any
// bootstrap exceptions (e.g. bad config) are also captured.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    // Suppress noisy framework namespaces in Serilog too
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()                          // add scope properties
                                                      // Console sink with a custom output template
                                                      .WriteTo.Console(
                                                          outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                                                          formatProvider: CultureInfo.InvariantCulture)
                                                      // File sink — writes a new file every day; keeps 7 days of logs
                                                      .WriteTo.File(
                                                          path: "logs/app-.log",
                                                          rollingInterval: RollingInterval.Day,
                                                          retainedFileCountLimit: 7,
                                                          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} — {Message:lj}{NewLine}{Exception}",
                                                          formatProvider: CultureInfo.InvariantCulture)
                                                      .CreateLogger();

try
{
    var serilogBuilder = Host.CreateApplicationBuilder(args);

    // AddSerilog() registers Serilog as the logging provider for all ILogger<T>
    // injections in this host, replacing the built-in providers.
    serilogBuilder.Services.AddSerilog();

    serilogBuilder.Services.AddSingleton<OrderService>();
    serilogBuilder.Services.AddSingleton<PaymentService>();
    serilogBuilder.Services.AddHostedService<SerilogLoggingDemo>();

    var serilogHost = serilogBuilder.Build();
    await serilogHost.RunAsync();
}
finally
{
    // Always flush and close Serilog at application exit to avoid losing
    // buffered log events (especially important for file/network sinks).
    await Log.CloseAndFlushAsync();
}

Console.WriteLine();
Console.WriteLine("✅ Logging demo complete. Check ./logs/ for the Serilog file output.");

// ─────────────────────────────────────────────────────────────────────────
// PART C — [LoggerMessage] source generator (high-performance logging)
// ─────────────────────────────────────────────────────────────────────────
Console.WriteLine();
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
Console.WriteLine("PART C — [LoggerMessage] source generator (high-performance)");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

var perfBuilder = Host.CreateApplicationBuilder(args);
perfBuilder.Logging.ClearProviders();
perfBuilder.Logging.AddConsole();
perfBuilder.Logging.AddFilter("Microsoft", LogLevel.Warning);
perfBuilder.Services.AddHostedService<HighPerfLoggingDemo>();

var perfHost = perfBuilder.Build();
await perfHost.RunAsync();

Console.WriteLine();
Console.WriteLine("✅ All logging demos complete.");
