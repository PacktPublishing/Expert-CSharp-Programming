var builder = DistributedApplication.CreateBuilder(args);

var dataStore = builder.AddParameter("DataStore");

var gameAPIs = builder.AddProject<Projects.Codebreaker_GameAPIs>("gameapis")
    .WithEnvironment("DataStore", dataStore);

builder.Build().Run();
