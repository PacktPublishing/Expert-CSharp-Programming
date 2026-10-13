# WpfMvvmToolkit — WPF Startup with Generic Host and MVVM Toolkit

This sample demonstrates a startup model for **WPF** that mirrors the chapter patterns:
- `Host.CreateApplicationBuilder(args)` in `App.xaml.cs`
- centralized DI, configuration, and logging
- `CommunityToolkit.Mvvm` for observable properties and relay commands
- constructor-injected `MainWindowViewModel`

## Prerequisites
- .NET 10 SDK
- Windows with WPF support
- .NET desktop development workload (Visual Studio)

## Run

```bash
cd WpfMvvmToolkit
dotnet run
```
