using BookStore.Api.Endpoints;
using BookStore.Api.Repositories;
using BookStore.Core.Repositories;
using BookStore.Core.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Register services using DI — IBookRepository and IBookService
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

// Enable OpenAPI / Swagger for development
builder.Services.AddOpenApi();

var app = builder.Build();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

// Map API endpoints
app.MapBooksEndpoints();

app.Run();
