var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.BookStore_Api>("api")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.BookStore_Web>("web")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
