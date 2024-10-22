# Chapter 2 - Generics under the hood

In this chapter, you need to have the .NET 9 SDK installed, and the *Metadata Tools* to check the IL code.

Currently, this tool is in preview and needs to be installed with a preview feed.

[Metadata Tools](https://github.com/dotnet/metadata-tools) can be installed from the prerelease source:

```bash
dotnet tool install mdv -g --prerelease --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json 
```

In this chapter, Visual Studio 2022 can be used to build and run the applications. For using the HTTP files with the features used in this chapter, at least version 17.12.0 is required.

- BehindTheScenes - a simple console application which helps understanding generics
- GamesSampleWithTuples - creating a generic interface using tuples
- GamesSampleWithRecords - the games sample with records instead of tuples
- NumericSample - implementing and using operators with generics
- AllowRefStruct - using the anti-constraint `allow ref struct`
