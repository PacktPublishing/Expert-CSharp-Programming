var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgreSQL("postgres")
    .WithDataVolume();

var postgresdb = postgres.AddDatabase("formula1db");

var blazorApp = builder.AddProject<Projects.Ch08_BlazorApp>("blazorapp")
    .WithReference(postgresdb);

builder.Build().Run();
