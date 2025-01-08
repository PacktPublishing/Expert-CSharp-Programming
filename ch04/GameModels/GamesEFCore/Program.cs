using GamesEFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<GamesContext>(options =>
{ 
    var connectionString = builder.Configuration.GetConnectionString("GamesConnection");
    options.UseSqlServer(connectionString);
});
builder.Services.AddTransient<Runner>();

using var host = builder.Build();

var runner = host.Services.GetRequiredService<Runner>();
await runner.CreateDatabaseAsync();
await runner.CreateAndStoreGame1Async();
await runner.CreateAndStoreGame2Async();
