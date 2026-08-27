using BackgroundServiceGC;

// ============================================================
// Background Service GC Demo
// Shows how to tune and monitor GC behavior in .NET worker
// services that run continuously in production environments.
// ============================================================

Console.OutputEncoding = System.Text.Encoding.UTF8;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register multiple background workers to show different GC patterns
        services.AddHostedService<MetricsCollectorWorker>();
        services.AddHostedService<DataProcessingWorker>();

        // Register shared object pools for GC pressure reduction
        services.AddSingleton<MessageBufferPool>();
    })
    .Build();

await host.RunAsync();
