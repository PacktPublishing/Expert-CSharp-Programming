using System.Diagnostics;

using BlazorClient.Client.Services;
using BlazorClient.Components;

using Books.Data;
using Books.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging();
builder.AddServiceDefaults();
builder.Services.AddHttpForwarderWithServiceDiscovery();
builder.Services.AddHttpClient<IBooksService, BooksClient>(
    client => client.BaseAddress = new Uri("https://booksapi"));

const string activitySourceName = "BooksClient";
const string activitySourceVersion = "1.0.0";

builder.Services.AddKeyedSingleton(activitySourceName, (services, _) =>
    new ActivitySource(activitySourceName, activitySourceVersion));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddDbContext<IBooksService, BooksContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("sqlite") ?? throw new InvalidOperationException("Connection string 'sqlite' not found");
    options.UseSqlite(connectionString);
});

var app = builder.Build();

app.UseRouting();
app.UseHttpLogging();

app.MapForwarder("/api/books/{**catch-all}", "https://booksapi", "/api/books/{**catch-all}"); 
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

// app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorClient.Client._Imports).Assembly);

app.Run();
