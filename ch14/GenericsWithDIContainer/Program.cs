using GenericsWithDIContainer;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient(typeof(GenericService<>));
builder.Services.AddTransient<Consumer>();

var host = builder.Build();
var consumer = host.Services.GetRequiredService<Consumer>();
consumer.Consume();
