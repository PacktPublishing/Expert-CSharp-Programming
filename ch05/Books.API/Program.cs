using Books.API.Endpoints;
using Books.Data;
using Books.Services;

using BooksService.Endpoints;

using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<IBooksService, BooksContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("sqlite") ?? throw new InvalidOperationException("Connection string 'sqlite' not found");
    options.UseSqlite(connectionString);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapDefaultEndpoints();

await app.CreateDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapBookEndpoints();

app.MapControllers().AddEndpointFilter<BookServiceExceptionFilter>();

app.Run();

static class WebApplicationExtensions
{
    public static async Task CreateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var booksService = scope.ServiceProvider.GetRequiredService<IBooksService>();
        if (booksService is BooksContext booksContext)
        {
            // Ensure the database is created
            var created = await booksContext.Database.EnsureCreatedAsync();
            app.Logger.LogInformation("Database {Status}", created ? "created" : "already exists");

            // Seed the database with sample books
            if (created)
            {
                var books = Enumerable.Range(1, 20)
                    .Select(index => new Book($"Title {index}", index, "Sample Pub"));
                booksContext.Books.AddRange(books);
                await booksContext.SaveChangesAsync();
            }
        }
    }
}

