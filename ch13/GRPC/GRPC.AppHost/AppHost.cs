// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

var builder = DistributedApplication.CreateBuilder(args);

var service = builder.AddProject<Projects.GRPCService>("grpcservice");

builder.AddProject<Projects.GRPCClient>("grpcclient")
    .WithReference(service)
    .WaitFor(service)
    .WithExplicitStart();

builder.Build().Run();
