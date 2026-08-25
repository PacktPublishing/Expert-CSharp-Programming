// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

var builder = DistributedApplication.CreateBuilder(args);
var sseService = builder.AddProject<Projects.SSE_Service>("sse-service");

builder.AddProject<Projects.SSE_Blazor>("sse-blazor-client")
    .WithReference(sseService).WaitFor(sseService);

builder.Build().Run();
