// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

// Program.cs — Configuration Providers & Options Pattern (Chapter 14)
//
// Demonstrates:
//   1. Default configuration stack and priority order
//   2. appsettings.json and appsettings.{Environment}.json
//   3. Environment-variable and command-line overrides
//   4. IOptions<T>         — snapshot at startup
//   5. IOptionsMonitor<T>  — live reload without restart
//   6. Options validation  — fail-fast on bad configuration
//   7. Raw IConfiguration  — direct key lookup

using Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ─── Build host ───────────────────────────────────────────────────────────
// Host.CreateApplicationBuilder() registers the following configuration
// sources in ascending priority order (last source wins for duplicate keys):
//
//   Priority  Source
//   ────────  ──────────────────────────────────────────────────────────────
//     1 (low)  appsettings.json
//     2        appsettings.{DOTNET_ENVIRONMENT}.json
//     3        User Secrets (Development environment only)
//     4        Environment variables  (keys added as-is; use "__" as hierarchy separator)
//     5 (high) Command-line arguments
//
// Override: DOTNET_ENVIRONMENT=Staging dotnet run
//           DATABASE__COMMANDTIMEOUTSECONDS=5 dotnet run
var builder = Host.CreateApplicationBuilder(args);

// ─── Bind strongly-typed options ──────────────────────────────────────────
// AddOptions<T>() chains fluent configuration methods:
//   .BindConfiguration()     — maps a JSON section → properties
//   .ValidateDataAnnotations() — honours [Required], [Range], [StringLength]
//   .ValidateOnStart()       — runs validation at startup, not first use
builder.Services.AddOptions<DatabaseOptions>()
    .BindConfiguration("Database")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<FeatureFlags>()
    .BindConfiguration("FeatureFlags")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// ─── Register the demo service ────────────────────────────────────────────
builder.Services.AddHostedService<ConfigDemoService>();

var host = builder.Build();
await host.RunAsync();

