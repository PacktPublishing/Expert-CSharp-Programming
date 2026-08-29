# AppStartup — Application Startup & Builder Patterns

This sample demonstrates **WebApplication builder patterns** in modern ASP.NET Core, including DI service registration, strongly-typed options, environment detection, middleware pipeline ordering, and minimal-API endpoint mapping.

## 🚀 Features Demonstrated

### 1. WebApplication Builder
The unified entry point for ASP.NET Core apps since .NET 6:
```csharp
var builder = WebApplication.CreateBuilder(args);
// configure services …
var app = builder.Build();
// configure middleware …
app.Run();
```

### 2. Strongly-Typed Options with Validation
Bind a config section to a typed class and validate it at startup:
```csharp
builder.Services.AddOptions<AppOptions>()
    .BindConfiguration("AppOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();   // crash early if config is missing or invalid
```

### 3. Service Lifetime Registration

| Lifetime | Method | Typical Use |
|---|---|---|
| Singleton | `AddSingleton<I, T>()` | Caches, stateless helpers |
| Scoped | `AddScoped<I, T>()` | EF DbContext, per-request state |
| Transient | `AddTransient<I, T>()` | Lightweight stateless services |

### 4. Environment Detection
```csharp
if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/error");
```

### 5. Configuration Layering
Values are merged in this priority order (last source wins):
1. `appsettings.json` (baseline)
2. `appsettings.{Environment}.json` (environment overrides)
3. Environment variables
4. Command-line arguments

### 6. Minimal API Endpoints
```csharp
app.MapGet("/config", (IOptions<AppOptions> opts) =>
    Results.Ok(new { opts.Value.Name, opts.Value.Version }));
```

## 📋 Prerequisites
- **.NET 10 SDK** or later

## 🔧 Run

```bash
cd ch14/AppStartup
dotnet run
# Override environment:
ASPNETCORE_ENVIRONMENT=Production dotnet run
```

Endpoints: `http://localhost:5100/`, `/time`, `/config`, `/environment`

## 📊 Sample Output
```
🌍 Starting in environment: Development
🚀 AppStartup sample is running. Try these endpoints:
   GET /            → greeting
   GET /time        → current UTC time
   GET /config      → app name & version from appsettings.json
   GET /environment → active environment info
```
