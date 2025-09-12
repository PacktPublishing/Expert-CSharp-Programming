using System.Diagnostics;
using System.Reflection;

using BlazorClient.Client.Services;

using Books.Services;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Service (logical) name shown in traces
const string serviceName = "BlazorClient.Wasm";

builder.Services.AddHttpClient<IBooksService, BooksClient>(
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddKeyedSingleton("BooksClientActivity", (_, _) =>
    new ActivitySource(typeof(Program).Assembly.GetName().Name!));

// OpenTelemetry(Tracing; limited Metrics in WASM)
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r
        .AddService(serviceName: serviceName,
                    serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown")
        .AddAttributes(
        [
            new KeyValuePair<string, object>("deployment.environment", builder.HostEnvironment.Environment),
            new KeyValuePair<string, object>("client.runtime", "webassembly")
        ]))
    .WithTracing(t =>
    {
        t.AddSource(serviceName)                  // manual spans
         .AddHttpClientInstrumentation(o =>
         {
             // Optional filtering
             o.FilterHttpRequestMessage = msg => !msg.RequestUri!.AbsolutePath.Contains("_framework");
         });

        // Sampler: AlwaysOn for dev; change to ParentBased+Ratio for production
        if (builder.HostEnvironment.IsDevelopment())
        {
            t.SetSampler(new AlwaysOnSampler());
        }

        // OTLP exporter (HTTP/protobuf). Point to a reachable public/gateway endpoint.
        var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        // string otlpEndpoint = "https://localhost:21035";
        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            t.AddOtlpExporter(o =>
            {
                o.Endpoint = new Uri(otlpEndpoint); // e.g. https://yourserver/otlp/v1/traces
                o.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
        }
    })
    // (Optional – runtime metrics; may be partially supported)
    .WithMetrics(m =>
    {
        m.AddRuntimeInstrumentation();
        // Add custom meters if you emit them
    });

await builder.Build().RunAsync();
