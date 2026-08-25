// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using SignalR.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");

app.MapHub<MarketHub>("/marketHub");

app.Run();
