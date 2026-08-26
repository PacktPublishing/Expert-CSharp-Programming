// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

var builder = DistributedApplication.CreateBuilder(args);

var service = builder.AddProject<Projects.SignalR_Service>("signalr-service");

builder.AddProject<Projects.SignalR_Blazor>("signalr-blazor")
    .WithReference(service).WaitFor(service);

builder.Build().Run();
