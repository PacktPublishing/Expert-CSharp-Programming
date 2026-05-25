# .NET 10 Upgrade

## Strategy
**Selected**: All-at-Once
**Rationale**: Single WPF project with straightforward .NET Framework → .NET 10 migration; no inter-project dependencies or phasing required. Atomic upgrade of project format and target framework, followed by breaking change resolution.

### Execution Constraints
- Single atomic upgrade — all changes applied together
- Validate full project build after each major step
- Address WPF platform-specific requirements (.NET 10 only available on Windows)
- 19 API issues identified: 12 binary incompatible, 2 source incompatible, 5 behavioral changes
- No external NuGet dependencies; may need System.Configuration.ConfigurationManager for legacy config compatibility

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: .NET 10 (net10.0) — LTS (Support ends Nov 2028)

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task
- **Branch Sync**: Auto (Merge)

## Key Decisions Log

### 2025-01-15: Scenario Initialized
- Confirmed target framework: .NET 10 (LTS)
- Confirmed working branch: upgrade-dotnet-10
- Confirmed commit strategy: After Each Task
- Repository root: D:\books\Expert-CSharp-Programming

### 2025-01-15: Planning Complete
- Generated plan with 4 tasks using All-at-Once strategy
- Assessment shows 19 API issues to resolve after framework update
- WPF support requires UseWindowsDesktop property or -windows TFM suffix
