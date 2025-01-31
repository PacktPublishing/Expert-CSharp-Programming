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

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var booksService = scope.ServiceProvider.GetRequiredService<IBooksService>();
    if (booksService is BooksContext booksContext)
    {
        var created = await booksContext.Database.EnsureCreatedAsync();
        if (app.Logger.IsEnabled(LogLevel.Information))
        {
            string logMessage = created ? "Database created" : "Database already exists";
            app.Logger.LogInformation(message: logMessage);
        }

        if (created)
        {
            var books = Enumerable.Range(1, 20)
                .Select(index => new Book($"Title {index}", index, "Sample Pub"));
            booksContext.Books.AddRange(books);
            await booksContext.SaveChangesAsync();
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

// app.UseHttpsRedirection();

app.MapBookEndpoints();

app.Run();
