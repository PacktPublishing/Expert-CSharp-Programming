using Ch08.BlazorApp.Components;
using Ch08.DataLib;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add SQL logging services
builder.Services.AddSingleton<SqlQueryLogger>();
builder.Services.AddSingleton<SqlLoggingInterceptor>();

builder.Services.AddDbContext<Formula1DataContext>((serviceProvider, optionsBuilder) =>
{
    var sqlInterceptor = serviceProvider.GetRequiredService<SqlLoggingInterceptor>();
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("formula1db"))
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

// Seed data on startup using the correct execution strategy
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Formula1DataContext>();
    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
        await Formula1DataSeeder.SeedDataAsync(context);
    });
}

app.Run();
