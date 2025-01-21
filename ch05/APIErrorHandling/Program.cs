using APIErrorHandling.Data;
using APIErrorHandling.Endpoints;

using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IBooksService, BooksContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BooksContext") ?? throw new InvalidOperationException("Connection string 'BooksContext' not found.")));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var booksService = scope.ServiceProvider.GetRequiredService<IBooksService>();
    if (booksService is BooksContext booksContext)
    {
        var created = await booksContext.Database.EnsureCreatedAsync();
        if (app.Logger.IsEnabled(LogLevel.Information))
        {
            string logMessage = created ? "Database created" : "Database already exists";
            app.Logger.LogInformation(logMessage);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();

app.MapBookEndpoints();

app.Run();
