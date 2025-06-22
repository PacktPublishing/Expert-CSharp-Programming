using Ch08.BlazorApp.Components;
using Ch08.DataLib;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add SQL logging services
builder.Services.AddSingleton<SqlQueryLogger>();
builder.Services.AddSingleton<SqlLoggingInterceptor>();

builder.Services.AddDbContext<Formula1DataContext>((serviceProvider, options) =>
{
    var sqlInterceptor = serviceProvider.GetRequiredService<SqlLoggingInterceptor>();
    options.UseNpgsql(builder.Configuration.GetConnectionString("formula1db"))
           .AddInterceptors(sqlInterceptor);
});

builder.EnrichNpgsqlDbContext<Formula1DataContext>();


// Add repository
builder.Services.AddScoped<IFormula1Repository, Formula1Repository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Seed data on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Formula1DataContext>();
    await Formula1DataSeeder.SeedDataAsync(context);
}

app.Run();
