// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Grpc.Core;

using GRPCClient;

using GRPCService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddTransient<Runner>();
builder.Services.AddGrpcClient<BookCatalog.BookCatalogClient>(options =>
{
    options.Address = new Uri("https://grpcservice");
});

var container = builder.Build();

string serviceAddress = "https://grpcservice";

try
{
    Console.WriteLine($"Connecting to {serviceAddress}");

    await Task.Delay(TimeSpan.FromSeconds(5));

    Runner runner = container.Services.GetRequiredService<Runner>();

    await runner.CallUnaryAsync();
    await runner.CallServerStreamingAsync();
    await runner.CallClientStreamingAsync();
    await runner.CallBidirectionalStreamingAsync();
}
catch (RpcException ex)
{
    Console.Error.WriteLine($"gRPC call failed ({ex.StatusCode}): {ex.Status.Detail}");
    Environment.ExitCode = 1;
}
catch (HttpRequestException ex)
{
    Console.Error.WriteLine($"Could not reach the gRPC service at {serviceAddress}: {ex.Message}");
    Environment.ExitCode = 1;
}
