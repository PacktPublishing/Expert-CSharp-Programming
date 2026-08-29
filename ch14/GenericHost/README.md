# GenericHost — Generic Host, Hosted Services & Lifetime Events

This sample demonstrates the **.NET Generic Host** for non-web applications — worker processes, console tools, and background services — using `IHostedService`, `BackgroundService`, and `IHostApplicationLifetime`.

## 🚀 Features Demonstrated

### 1. Host.CreateApplicationBuilder()
The modern, low-ceremony way to bootstrap a non-web host:
```csharp
var builder = Host.CreateApplicationBuilder(args);
// register services …
var host = builder.Build();
await host.RunAsync();
```

### 2. IHostedService — One-Shot Startup Logic
Implement `StartAsync` / `StopAsync` for operations that run exactly once:
```csharp
sealed class StartupReportService(IHostEnvironment env) : IHostedService
{
    public Task StartAsync(CancellationToken ct)
    {
        // warm cache, run DB migration check, log startup info …
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
```

### 3. BackgroundService — Long-Running Loop
`BackgroundService` wraps `IHostedService` and simplifies the loop pattern:
```csharp
sealed class HeartbeatService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            DoWork();
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
```

### 4. Host Lifetime Events
Subscribe to lifecycle callbacks via `IHostApplicationLifetime`:
```csharp
lifetime.ApplicationStarted.Register(()  => Console.WriteLine("Started"));
lifetime.ApplicationStopping.Register(() => Console.WriteLine("Stopping"));
lifetime.ApplicationStopped.Register(()  => Console.WriteLine("Stopped"));
```

### 5. Graceful Shutdown
Call `StopApplication()` to initiate an orderly shutdown:
```csharp
lifetime.StopApplication(); // cancels stoppingToken for all hosted services
```
Or press **Ctrl+C** — the host handles SIGINT/SIGTERM automatically.

### 6. Service Start / Stop Ordering
Hosted services are started in **registration order** and stopped in **reverse order**:
```csharp
builder.Services.AddHostedService<StartupReportService>(); // starts first
builder.Services.AddHostedService<HeartbeatService>();     // starts second
// On shutdown: HeartbeatService stops first, StartupReportService stops last
```

## 📋 Prerequisites
- **.NET 10 SDK** or later

## 🔧 Run

```bash
cd ch14/GenericHost
dotnet run
```

Press **Ctrl+C** at any time to test graceful shutdown.

## 📊 Sample Output

```
🚀 GenericHost Sample — Hosted Services & Lifetime Events
===========================================================
info: StartupReportService[0]
      📋 Application starting | Environment: Production | App: GenericHost | Root: …
⚡ [Lifetime] ApplicationStarted  — all hosted services are running
info: HeartbeatService[0]
      💓 HeartbeatService started — will run 5 heartbeats
info: HeartbeatService[0]
      💓 Heartbeat 1/5 | Total recorded: 1
info: HeartbeatService[0]
      💓 Heartbeat 2/5 | Total recorded: 2
…
info: HeartbeatService[0]
      💓 HeartbeatService finished all heartbeats — requesting graceful shutdown
⚡ [Lifetime] ApplicationStopping — graceful shutdown initiated
⚡ [Lifetime] ApplicationStopped  — all hosted services have stopped
✅ Host has shut down cleanly.
```
