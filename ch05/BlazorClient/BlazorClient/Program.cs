using BlazorClient.Components;

using Books.Data;
using Books.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpLogging();
builder.AddServiceDefaults();
// builder.Services.AddReverseProxy().AddServiceDiscoveryDestinationResolver();

builder.Services.AddHttpForwarderWithServiceDiscovery();

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


app.MapForwarder("/apibooks/{**catch-all}", "https://booksapi", "/api/books/{**catch-all}");
app.MapForwarder("/abc", "https://booksapi", "/api/books");  
app.MapDefaultEndpoints();
// app.MapReverseProxy();

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
