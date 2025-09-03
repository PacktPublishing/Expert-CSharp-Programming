var builder = DistributedApplication.CreateBuilder(args);

//var db = builder.AddMySql("mysql")
//    .WithPhpMyAdmin()
//    .AddDatabase("booksdb");

var db = builder.AddSqlite("sqlite")
    .WithSqliteWeb();

var api = builder.AddProject<Projects.Books_API>("booksapi")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.BlazorClient>("blazorclient")
    //.AddWebAssemblyClient<Projects.BlazorClient.Client>("blazorwasm")
    .WithReference(db)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
