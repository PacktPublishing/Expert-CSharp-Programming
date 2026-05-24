# 03-resolve-breaking-changes: Fix API incompatibilities and breaking changes

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
