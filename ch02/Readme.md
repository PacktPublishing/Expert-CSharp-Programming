# Chapter 2 - Generics under the hood

In this chapter, you need to have the .NET 10 SDK installed, and the *Metadata Tools* to check the IL code.

Currently, this tool is in preview and needs to be installed with a preview feed.

[Metadata Tools](https://github.com/dotnet/metadata-tools) can be installed from the prerelease source:

```bash
dotnet tool install mdv -g --prerelease --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json 
```

In this chapter, Visual Studio 2022 can be used to build and run the applications.

- BehindTheScenes - a simple console application which helps understanding generics
- GamesSample - creating a generic interface, generic classes, and generic methods, and using record structs and tuples as generic parameters
- NumericSample - implementing and using operators with generics
- AllowRefStruct - using the anti-constraint `allow ref struct`

> The GamesSample project uses the conditional compilation symbol USERECORDS. If this compilation symbol is set in the project file, a C# record struct is used for the ShapeResult type. If this symbol is not set, tuples are used instead.

[Class diagram game classes](GameClasses.md)
