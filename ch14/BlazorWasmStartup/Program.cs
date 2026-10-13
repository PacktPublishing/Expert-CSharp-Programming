// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using BlazorWasmStartup;
using BlazorWasmStartup.Services;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddOptions<ClientAppOptions>()
    .Bind(builder.Configuration.GetSection("ClientApp"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<IGreetingClient, GreetingClient>();

builder.Logging.SetMinimumLevel(LogLevel.Information);

var host = builder.Build();

var startupLogger = host.Services
    .GetRequiredService<ILoggerFactory>()
    .CreateLogger("BlazorWasmStartup");

var options = host.Services.GetRequiredService<IOptions<ClientAppOptions>>().Value;

startupLogger.LogInformation(
    "Starting Blazor WASM host | App: {AppName} | Version: {Version}",
    options.Name,
    options.Version);

await builder.Build().RunAsync();
