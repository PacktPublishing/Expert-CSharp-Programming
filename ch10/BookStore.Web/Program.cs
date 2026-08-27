using BookStore.Web;
using BookStore.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Blazor Server services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<BooksClient>(client =>
{
    client.BaseAddress = new Uri("https+http://api");
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

