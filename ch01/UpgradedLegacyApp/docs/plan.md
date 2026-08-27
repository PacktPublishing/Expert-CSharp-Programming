# .NET 10 Upgrade Plan

## Overview

**Target**: Upgrade LiveShaping WPF desktop application from .NET Framework 4.8 to .NET 10
**Scope**: Single WPF project, 408 LOC, no external dependencies

## Strategy

**Selected**: All-at-Once
**Rationale**: Single project with straightforward .NET Framework → .NET 10 migration; no inter-project dependencies or phasing required

### Execution Constraints
- Single atomic upgrade — all changes applied together
- Validate full project build after each major step
- Address WPF platform-specific requirements (.NET 10 only available on Windows)
- No breaking changes in legacy configuration system — use System.Configuration.ConfigurationManager as interim bridge if needed

---

## Tasks

### 01-convert-to-sdk-style: Convert project to SDK-style format

The LiveShaping.csproj currently uses the legacy .NET Framework project format (old csproj with ToolsVersion attribute). Modern .NET requires conversion to SDK-style format with simplified project structure.

This task includes:
- Convert project file structure to SDK format
- Update or remove legacy property groups (AssemblyInfo, resource embeddings)
- Validate project loads and builds correctly in Visual Studio

**Done when**: 
- Project loads in IDE without errors
- Project builds successfully as SDK-style
- No build errors or warnings introduced

---

### 02-update-target-framework: Update target framework to .NET 10

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

---

### 03-resolve-breaking-changes: Fix API incompatibilities and breaking changes

Address the 19 API issues identified in the assessment:

**WPF/Windows APIs** (10 issues):
- FrameworkElement.DataContext usage changes
- Window constructor and Application lifecycle APIs
- IComponentConnector XAML loader changes
- StartupUri and Application.Run patterns

Research points: Review MainWindow.xaml.cs and App.xaml.cs for legacy WPF patterns; check for type-forwarding issues.

**Configuration System** (2 issues):
- ApplicationSettingsBase usage in Settings.Designer.cs
- Legacy XML configuration patterns — may need Microsoft.Extensions.Configuration package reference

Research points: Verify App.config is used correctly; ApplicationSettingsBase may need System.Configuration.ConfigurationManager NuGet package.

**System.Uri** (5 issues):
- Behavioral changes in Uri constructor (System.Uri and System.Uri(string, UriKind))
- May affect any URL/path parsing in Formula1.cs, LapChart.cs, or LapRacerInfo.cs

Research points: Check for any Uri construction calls with hardcoded schemes or relative path handling.

**Done when**:
- All compilation errors resolved
- Project builds warning-free
- Existing functionality preserved (no test regressions)

---

### 04-validate-upgrade: Final testing and validation

Build the complete solution and verify functionality. Run any existing unit tests if present; perform manual smoke testing of the WPF UI to ensure data binding and window initialization work correctly.

**Done when**:
- Project builds successfully with no errors or warnings
- All unit tests pass (if applicable)
- Manual UI testing confirms application launches and responds to user input
- No unresolved API incompatibilities
