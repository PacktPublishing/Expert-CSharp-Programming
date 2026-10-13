# Configuration — Configuration Providers & Options Pattern

This sample demonstrates the full **.NET configuration stack** — layered providers, strongly-typed options, validation, live-reload, and environment-variable / command-line overrides.

## 🚀 Features Demonstrated

### 1. Configuration Source Priority
Sources are registered in this order; **later sources override earlier ones**:

| Priority | Source |
|---|---|
| 1 (lowest) | `appsettings.json` |
| 2 | `appsettings.{Environment}.json` |
| 3 | User Secrets (Development only) |
| 4 | Environment variables |
| 5 (highest) | Command-line arguments |

### 2. Strongly-Typed Options with Validation
```csharp
builder.Services.AddOptions<DatabaseOptions>()
    .BindConfiguration("Database")        // JSON section → C# class
    .ValidateDataAnnotations()            // [Required], [Range], etc.
    .ValidateOnStart();                   // fail at startup, not at runtime
```

### 3. IOptions\<T\> — Startup Snapshot
Singleton; value is computed once at startup:
```csharp
sealed class MyService(IOptions<DatabaseOptions> opts)
{
    string conn = opts.Value.ConnectionString;
}
```

### 4. IOptionsMonitor\<T\> — Live Reload
Re-reads configuration when `appsettings.json` changes on disk:
```csharp
sealed class MyService(IOptionsMonitor<FeatureFlags> monitor)
{
    bool isBeta = monitor.CurrentValue.EnableBetaFeatures;
}
// Register a change callback:
monitor.OnChange(flags => Console.WriteLine($"Reloaded: beta={flags.EnableBetaFeatures}"));
```

### 5. Environment-Variable Override
Use `__` as the hierarchy separator (works on all platforms):
```bash
DATABASE__COMMANDTIMEOUTSECONDS=99 dotnet run
```

### 6. Command-Line Override
```bash
dotnet run -- --Database:CommandTimeoutSeconds=5
```

## 📋 Prerequisites
- **.NET 10 SDK** or later

## 🔧 Run

```bash
cd ch14/Configuration
dotnet run

# Run with Production settings (uses appsettings.json only, not .Development.json)
DOTNET_ENVIRONMENT=Production dotnet run

# Override a value at runtime
DATABASE__COMMANDTIMEOUTSECONDS=5 dotnet run
```

## 📊 Sample Output

```
🔧 Configuration Providers & Options Pattern
============================================

1️⃣  Raw IConfiguration — direct key lookup
   AppName                      = ConfigurationSample
   Database:ConnectionString    = Server=localhost;Database=myapp_dev;…
   Database:CommandTimeoutSeconds = 60

2️⃣  IOptions<DatabaseOptions> — startup snapshot
   ConnectionString        = Server=localhost;Database=myapp_dev;…
   CommandTimeoutSeconds   = 60
   EnableRetry             = True
   MaxRetryCount           = 3

3️⃣  IOptionsMonitor<FeatureFlags> — live reload
   EnableDarkMode          = False
   EnableBetaFeatures      = True
   MaxUploadSizeMb         = 100
   (Change callback registered — edit appsettings.json to see live reload)
…
✅ Configuration demo complete.
```
