using System.Diagnostics;

using BlazorClient.Client.Services;

using Books.Services;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddHttpClient<IBooksService, BooksClient>(
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));


const string activitySourceName = "BooksClient";
const string activitySourceVersion = "1.0.0";

builder.Services.AddKeyedSingleton(activitySourceName, (services, _) =>
    new ActivitySource(activitySourceName, activitySourceVersion));

await builder.Build().RunAsync();
