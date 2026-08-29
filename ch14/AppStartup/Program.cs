// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using AppStartup;

using Microsoft.Extensions.Options;

// Program.cs — Application Startup & Builder Patterns (Chapter 14)
//
// Demonstrates:
//   1. WebApplication.CreateBuilder — the modern unified entry point
//   2. Service registration (DI) with different lifetimes
//   3. Strongly-typed options bound from configuration
//   4. Environment detection and environment-specific behaviour
//   5. Middleware pipeline ordering
//   6. Minimal API endpoint mapping

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ─── 1. Create the builder ────────────────────────────────────────────────
// WebApplication.CreateBuilder() wires up a complete host including:
//   • Default configuration stack (appsettings.json → appsettings.{env}.json
//     → environment variables → command-line args)
//   • Kestrel HTTP server
//   • Built-in DI container
//   • Console + Debug logging
//   • IWebHostEnvironment for environment detection
var builder = WebApplication.CreateBuilder(args);

// ─── 2. Bind strongly-typed options ──────────────────────────────────────
// Prefer IOptions<T> over raw IConfiguration — it gives compile-time safety,
// validation hooks, and hot-reload support (IOptionsMonitor<T>).
builder.Services.AddOptions<AppOptions>()
    .BindConfiguration("AppOptions")        // reads the "AppOptions" JSON section
    .ValidateDataAnnotations()              // honours [Required], [Range], etc.
    .ValidateOnStart();                     // fail fast — crash at startup rather than at runtime

// ─── 3. Register application services ────────────────────────────────────
// Singleton — one instance for the lifetime of the application
builder.Services.AddSingleton<IGreeterService, GreeterService>();

// Scoped — one instance per HTTP request (default for EF DbContext, etc.)
builder.Services.AddScoped<IOrderRepository, SqlOrderRepository>();

// Transient — a new instance every time it is injected
builder.Services.AddTransient<IDateTimeProvider, UtcDateTimeProvider>();

// ─── 4. Environment-aware registration ───────────────────────────────────
// Register extra tooling only in Development to keep production lean.
if (builder.Environment.IsDevelopment())
{
    // API explorer feeds Swagger/OpenAPI UI
    builder.Services.AddEndpointsApiExplorer();
}

Console.WriteLine($"🌍 Starting in environment: {builder.Environment.EnvironmentName}");

// ─── 5. Build the application ─────────────────────────────────────────────
var app = builder.Build();

// ─── 6. Configure the HTTP middleware pipeline ────────────────────────────
// ORDER MATTERS — each request passes through middleware top-to-bottom.
if (app.Environment.IsDevelopment())
{
    // Show rich error details only in Development (never in Production).
    app.UseDeveloperExceptionPage();
}
else
{
    // In Production: a generic error handler avoids leaking internal details.
    app.UseExceptionHandler("/error");
    app.UseHsts(); // HTTP Strict Transport Security
}

// custom middleware
app.Use(async (context, next) =>
{
    context.Response.Headers["X-App-Name"] = "AppStartup";
    await next();
});

// ─── 7. Map minimal-API endpoints ─────────────────────────────────────────
// Minimal APIs are concise and performant; no controller class required.
app.MapGet("/", (IGreeterService greeter) =>
    TypedResults.Ok(greeter.Greet("World")));

app.MapGet("/time", (IDateTimeProvider clock) =>
    TypedResults.Ok(new { utc = clock.UtcNow }));

// Read strongly-typed options via IOptions<T>
app.MapGet("/config", (IOptions<AppOptions> opts) =>
{
    AppOptions o = opts.Value;
    // Only expose non-sensitive configuration values
    return TypedResults.Ok(new { o.Name, o.Version, o.MaxItems });
});

app.MapGet("/environment", (IWebHostEnvironment env) =>
    TypedResults.Ok(new
    {
        name = env.EnvironmentName,
        isDevelopment = env.IsDevelopment(),
        isStaging = env.IsStaging(),
        isProduction = env.IsProduction(),
    }));

app.MapGet("/orders", async (IOrderRepository repo) =>
    TypedResults.Ok(await repo.GetAllAsync()));

app.MapGet("/orders/{id:int}", async (int id, IOrderRepository repo) =>
    await repo.GetByIdAsync(id) is Order order
        ? TypedResults.Ok(order)
        : Results.NotFound());

app.MapPost("/orders", async (Order order, IOrderRepository repo) =>
{
    Order saved = await repo.AddAsync(order);
    return TypedResults.Created($"/orders/{saved.Id}", saved);
});

app.MapGet("/error", () =>
    TypedResults.Problem("An unexpected error occurred."));

Console.WriteLine("🚀 AppStartup sample is running. Try these endpoints:");
Console.WriteLine("GET  /            → greeting");
Console.WriteLine("GET  /time        → current UTC time");
Console.WriteLine("GET  /config      → app name & version from appsettings.json");
Console.WriteLine("GET  /environment → active environment info");
Console.WriteLine("GET  /orders      → list all orders");
Console.WriteLine("GET  /orders/{id} → get order by id");
Console.WriteLine("POST /orders      → add a new order");
Console.WriteLine();

app.Run();
