# 02-update-target-framework: Update target framework to .NET 10

Change the project's target framework from `net48` to `net10.0` and enable Windows Desktop support for WPF.

Assessment identified 19 API-related issues that will become apparent after framework upgrade:
- 12 binary incompatible APIs (mostly WPF and System.Uri related)
- 2 source incompatible APIs (System.Configuration.ApplicationSettingsBase usage)
- 5 behavioral changes in System.Uri and System.Windows APIs

This task focuses on the framework change itself; API resolution happens in the next task.

**Done when**:
- Project file targets `net10.0`
- Project includes `<UseWindowsDesktop>true</UseWindowsDesktop>` or `-windows` TFM suffix for WPF support
- Project file structure is valid (validation via IDE)
