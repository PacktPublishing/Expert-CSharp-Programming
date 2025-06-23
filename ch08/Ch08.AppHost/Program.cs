using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var dataStore = builder.Configuration["DataStore"] ?? "PostgreSQL";

var blazorApp = builder.AddProject<Projects.Ch08_BlazorApp>("blazorapp")
    .WithEnvironment("DataStore", dataStore);

if (dataStore == "PostgreSQL")
{
    var postgres = builder.AddPostgres("postgres")
        .WithDataVolume("formula1-postgres-data")
        .WithPgWeb();

    var postgresdb = postgres.AddDatabase("formula1db");

    blazorApp.WithReference(postgresdb)
        .WaitFor(postgresdb);
}
else if (builder.Configuration["DataStore"] == "SqlServer")
{
    var sqlServer = builder.AddSqlServer("sqlserver")
        .WithDataVolume("formula1-sql-data");
    var sqldb = sqlServer.AddDatabase("formula1db");

    blazorApp.WithReference(sqldb)
        .WaitFor(sqldb);
}
else
{
    throw new InvalidOperationException("Unsupported data store specified in configuration.");
}

builder.Build().Run();
