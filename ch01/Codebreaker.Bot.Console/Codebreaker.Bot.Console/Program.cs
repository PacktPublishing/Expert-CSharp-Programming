using Codebreaker.GameAPIs.Client;

using CodeBreaker.Bot;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

int parallel = ParseValueFromArgs(nameof(parallel), 3, args);
int count = ParseValueFromArgs(nameof(count), 5, args);
int gameWait = ParseValueFromArgs(nameof(gameWait).ToLower(), 1, args);
int thinkTime = ParseValueFromArgs(nameof(thinkTime).ToLower(), 0, args);

// Rest of the code...

Console.WriteLine($"Threads : {parallel}");
Console.WriteLine($"Count: {count}");
Console.WriteLine($"Game wait: {gameWait}");
Console.WriteLine($"Think time: {thinkTime}");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<CodeBreakerTimer>();
builder.Services.AddSingleton<CodeBreaker6x4GameRunner>();
builder.Services.AddHttpClient<GamesClient>(static client =>
{
    client.BaseAddress = new Uri("https://localhost:9401");
});

using var host = builder.Build();


var timer = host.Services.GetRequiredService<CodeBreakerTimer>();
Guid id = timer.Start(parallel, gameWait, count, thinkTime);

Console.WriteLine($"Timer started with id {id}");
Console.WriteLine("Press return to exit");
Console.ReadLine();
Console.WriteLine("Bye!");

static int ParseValueFromArgs(string keyword, int defaultValue, string[] args)
{
    foreach (string arg in args)
    {
        if (arg.StartsWith($"--{keyword}="))
        {
            string countValue = arg[$"--{keyword}=".Length..];
            return int.TryParse(countValue, out int var) ? var : defaultValue;
        }
    }
    return defaultValue;
}
