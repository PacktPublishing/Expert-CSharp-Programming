using System.Runtime.CompilerServices;

using Codebreaker.Data.SqlServer;
using Codebreaker.GameAPIs;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

[assembly: InternalsVisibleTo("Codbreaker.APIs.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Swagger/EndpointDocumentation
builder.Services.AddEndpointsApiExplorer();

// Application Services

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (builder.Environment.IsDevelopment() && builder.Configuration["DataStore"] == "SqlServer")
{
    try
    {
        using var scope = app.Services.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<GamesSqlServerContext>();
        if (repo is GamesSqlServerContext context)
        {
            await context.Database.MigrateAsync();
            app.Logger.LogInformation("Database updated");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error updating database");
    }
}

app.MapGameEndpoints();

app.Run();
