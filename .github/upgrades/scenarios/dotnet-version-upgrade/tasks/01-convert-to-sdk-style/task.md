# 01-convert-to-sdk-style: Convert project to SDK-style format

The LiveShaping.csproj currently uses the legacy .NET Framework project format (old csproj with ToolsVersion attribute). Modern .NET requires conversion to SDK-style format with simplified project structure.

This task includes:
- Convert project file structure to SDK format
- Update or remove legacy property groups (AssemblyInfo, resource embeddings)
- Validate project loads and builds correctly in Visual Studio

**Done when**: 
- Project loads in IDE without errors
- Project builds successfully as SDK-style
- No build errors or warnings introduced
