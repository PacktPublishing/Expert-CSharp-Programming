using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("formula1db");

var blazorApp = builder.AddProject<Projects.Ch08_BlazorApp>("blazorapp")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.Build().Run();
