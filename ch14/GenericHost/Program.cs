// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

// Program.cs — Generic Host, Hosted Services & Host Lifetime (Chapter 14)
//
// Demonstrates:
//   1. Host.CreateApplicationBuilder() — the entry point for non-web apps
//   2. IHostedService       — one-shot startup / teardown logic
//   3. BackgroundService    — long-running background work loop
//   4. IHostApplicationLifetime — subscribe to host lifetime events
//   5. Graceful shutdown    — StopApplication() and CancellationToken

using GenericHost;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("🚀 GenericHost Sample — Hosted Services & Lifetime Events");
Console.WriteLine("===========================================================");

// ─── 1. Create the application builder ───────────────────────────────────
// Host.CreateApplicationBuilder() bootstraps a non-web host with:
//   • DI container, configuration, and logging pre-wired
//   • Default configuration stack (appsettings → env vars → CLI args)
//   • Console + Debug logging providers
//   • Signal handling (Ctrl+C → graceful shutdown)
var builder = Host.CreateApplicationBuilder(args);

// ─── 2. Register application services ────────────────────────────────────
// Shared state the hosted services can use via DI
builder.Services.AddSingleton<IMetricsCollector, InMemoryMetricsCollector>();

// ─── 3. Register hosted services ─────────────────────────────────────────
// The host starts each IHostedService in registration order and stops them
// in reverse order during shutdown — giving a clean teardown sequence.
builder.Services.AddHostedService<StartupReportService>(); // runs once at startup
builder.Services.AddHostedService<HeartbeatService>();     // periodic background loop

// ─── 4. Build the host ────────────────────────────────────────────────────
var host = builder.Build();

// ─── 5. Subscribe to host lifetime events ─────────────────────────────────
// IHostApplicationLifetime exposes three CancellationTokens that fire at key
// host lifecycle moments. Register handlers before calling Run/RunAsync.
var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() =>
    Console.WriteLine("[Lifetime] ApplicationStarted  — all hosted services are running"));

lifetime.ApplicationStopping.Register(() =>
    Console.WriteLine("[Lifetime] ApplicationStopping — graceful shutdown initiated"));

lifetime.ApplicationStopped.Register(() =>
    Console.WriteLine("[Lifetime] ApplicationStopped  — all hosted services have stopped"));

// RunAsync() starts all hosted services and waits until the host is stopped.
// Press Ctrl+C at any time to trigger graceful shutdown.
await host.RunAsync();

Console.WriteLine("Host has shut down cleanly.");

