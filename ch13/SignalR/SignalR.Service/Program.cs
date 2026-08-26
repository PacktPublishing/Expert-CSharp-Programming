// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using SignalR.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.AddServiceDefaults();
var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Use a SignalR client to access this service");

app.MapHub<MarketHub>("/marketHub");

app.Run();
