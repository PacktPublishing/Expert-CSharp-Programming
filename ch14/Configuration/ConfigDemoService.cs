// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Configuration;

// ─────────────────────────────────────────────────────────────────────────
// ConfigDemoService — shows each configuration access pattern
// ─────────────────────────────────────────────────────────────────────────

public sealed class ConfigDemoService(
    IConfiguration rawConfig,       // direct key lookup, no type safety
    IOptions<DatabaseOptions> dbOptions,   // snapshot — frozen at startup
    IOptionsMonitor<FeatureFlags> flagsMon, // live-reload — reflects file changes
    ILogger<ConfigDemoService> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("🔧 Configuration Providers & Options Pattern");
        Console.WriteLine("============================================");
        Console.WriteLine();

        // ── Section 1: Raw IConfiguration ─────────────────────────────────
        // Good for one-off reads; prefer IOptions<T> for regular access.
        Console.WriteLine("1️⃣  Raw IConfiguration — direct key lookup");
        Console.WriteLine("   Keys use ':' as the hierarchy separator.");
        string? appName = rawConfig["AppName"];               // top-level key
        string? connStr = rawConfig["Database:ConnectionString"]; // nested key
        Console.WriteLine($"   AppName                     = {appName}");
        Console.WriteLine($"   Database:ConnectionString    = {connStr}");

        // GetSection returns a sub-tree; GetValue<T> applies type conversion.
        int timeout = rawConfig.GetSection("Database")
                               .GetValue<int>("CommandTimeoutSeconds");
        Console.WriteLine($"   Database:CommandTimeoutSeconds = {timeout}");
        Console.WriteLine();

        // ── Section 2: IOptions<T> — startup snapshot ─────────────────────
        // IOptions<T> is a singleton; its .Value is computed once and cached.
        // Best for settings that never change while the app is running.
        Console.WriteLine("2️⃣  IOptions<DatabaseOptions> — startup snapshot");
        DatabaseOptions db = dbOptions.Value;
        Console.WriteLine($"   ConnectionString        = {db.ConnectionString}");
        Console.WriteLine($"   CommandTimeoutSeconds   = {db.CommandTimeoutSeconds}");
        Console.WriteLine($"   EnableRetry             = {db.EnableRetry}");
        Console.WriteLine($"   MaxRetryCount           = {db.MaxRetryCount}");
        Console.WriteLine();

        // ── Section 3: IOptionsMonitor<T> — live-reload ───────────────────
        // IOptionsMonitor<T> re-reads from configuration whenever the source
        // file changes. Access CurrentValue for the current snapshot.
        Console.WriteLine("3️⃣  IOptionsMonitor<FeatureFlags> — live reload");
        FeatureFlags flags = flagsMon.CurrentValue;
        Console.WriteLine($"   EnableDarkMode          = {flags.EnableDarkMode}");
        Console.WriteLine($"   EnableBetaFeatures      = {flags.EnableBetaFeatures}");
        Console.WriteLine($"   MaxUploadSizeMb         = {flags.MaxUploadSizeMb}");

        // Register a change callback — fires when appsettings.json is saved.
        flagsMon.OnChange(updated =>
            logger.LogInformation(
                "🔄 FeatureFlags reloaded — EnableBetaFeatures: {Beta}",
                updated.EnableBetaFeatures));
        Console.WriteLine("   (Change callback registered — edit appsettings.json to see live reload)");
        Console.WriteLine();

        // ── Section 4: Environment variable overrides ─────────────────────
        // The host maps env-var key separators: "__" → ":" (cross-platform).
        // Example:
        //   DATABASE__COMMANDTIMEOUTSECONDS=99 dotnet run
        //   → overwrites Database:CommandTimeoutSeconds in IConfiguration
        Console.WriteLine("4️⃣  Environment-variable override tip");
        Console.WriteLine("   Set  DATABASE__COMMANDTIMEOUTSECONDS=99  before running");
        Console.WriteLine("   to override Database:CommandTimeoutSeconds at runtime.");
        Console.WriteLine();

        // ── Section 5: Command-line override ──────────────────────────────
        // dotnet run -- --Database:CommandTimeoutSeconds=5
        Console.WriteLine("5️⃣  Command-line override tip");
        Console.WriteLine("   Run: dotnet run -- --Database:CommandTimeoutSeconds=5");
        Console.WriteLine("   Command-line args have the highest priority.");
        Console.WriteLine();

        Console.WriteLine("✅ Configuration demo ready. Press Ctrl+C to exit.");
        Console.WriteLine("   While running, try changing 'EnableBetaFeatures' in appsettings.json");
        Console.WriteLine("   and save — IOptionsMonitor will log the updated value without a restart.");
        // Keep the host running so the IOptionsMonitor.OnChange callback can fire.
        // Press Ctrl+C (or send SIGTERM) to trigger graceful shutdown.
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Configuration demo stopping.");
        return Task.CompletedTask;
    }
}
