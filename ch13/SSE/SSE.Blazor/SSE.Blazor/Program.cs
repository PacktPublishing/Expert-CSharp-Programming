using SSE.Blazor.Client.Pages;
using SSE.Blazor.Components;

using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHttpForwarderWithServiceDiscovery();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

// Forward /api/** calls to SSE.Service, stripping the /api prefix.
// ActivityTimeout = null keeps the connection alive for long-lived SSE streams.
var sseForwarderConfig = new ForwarderRequestConfig { ActivityTimeout = null };
app.MapForwarder("/api/{**catch-all}", "https://sse-service", sseForwarderConfig, new StripPrefixTransformer("/api"));

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SSE.Blazor.Client._Imports).Assembly);

app.Run();

/// <summary>Strips a prefix segment from the forwarded request path.</summary>
sealed class StripPrefixTransformer(string prefix) : HttpTransformer
{
    public override async ValueTask TransformRequestAsync(
        HttpContext httpContext,
        HttpRequestMessage proxyRequest,
        string destinationPrefix,
        CancellationToken cancellationToken)
    {
        await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);

        string path = httpContext.Request.Path.Value ?? string.Empty;
        if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            path = path[prefix.Length..];
        }

        var uri = RequestUtilities.MakeDestinationAddress(
            destinationPrefix,
            new PathString(path),
            httpContext.Request.QueryString);

        proxyRequest.RequestUri = uri;
    }
}
