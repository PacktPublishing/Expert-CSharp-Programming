using Ch08.BlazorApp.Components;
using Ch08.DataLib;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add SQL logging services
builder.Services.AddSingleton<SqlQueryLogger>();
builder.Services.AddSingleton<SqlLoggingInterceptor>();

var dataStore = builder.Configuration["DataStore"] ??= "PostgreSQL";

if (dataStore == "PostgreSQL")
{
    builder.Services.AddDbContextFactory<Formula1DataContext>((services, optionsBuilder) =>
    {
        var sqlInterceptor = services.GetRequiredService<SqlLoggingInterceptor>();
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("formula1db"))
            .AddInterceptors(sqlInterceptor)
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    });

    builder.EnrichNpgsqlDbContext<Formula1DataContext>();
}
else if (dataStore == "SqlServer")
{
    builder.Services.AddDbContextFactory<Formula1DataContext>((services, optionsBuilder) =>
    {
        var sqlInterceptor = services.GetRequiredService<SqlLoggingInterceptor>();
        optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("formula1db"))
            .AddInterceptors(sqlInterceptor)
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    });
    builder.EnrichSqlServerDbContext<Formula1DataContext>();

}
else
{
    throw new InvalidOperationException("Unsupported data store specified in configuration.");
}


// Add repository as scoped service
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

// Seed data on startup using DbContextFactory
using (var scope = app.Services.CreateScope())
{
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Formula1DataContext>>();
    await using var context = await contextFactory.CreateDbContextAsync();
    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
        await Formula1DataSeeder.SeedDataAsync(context);
    });
}

app.Run();
