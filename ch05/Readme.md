# Chapter 5 - Effective error handling and logging

This chapter requires the .NET SDK to be installed – for the TODO sample, either Docker Desktop or podman is needed to run the .NET Aspire application. Check the readme file of this chapter for details. The ch05 source code folder of the https://github.com/PacktPublishing/Expert-CSharp-Programming repository contains the code samples for this chapter.

See [setup tooling](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling) to run .NET Aspire on Windows or Linux, with Visual Studio, Visual Studio Code, and the .NET CLI.

The projects of this chapter are:

- Exceptions - foundations for using Exceptions
- ExceptionBenchmark - a benchmark program to compare code results versus exception handling
- Books App
  - Books.AppHost - the .NET Aspire AppHost - use this to start the application
  - Books.Data - a library using EF Core
  - Books.ServiceDefaults - a common library for .NET Aspire extensions
  - Books.API - the API service showing error handling
  - Books.Client - a Blazor client application accessing the API service
