
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

int result = Add(3, 4);
Console.WriteLine($"The result of 3 and 4 is {result}");

// Uncomment the following lines to show the target framework and runtime information
Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"Runtime: {RuntimeInformation.RuntimeIdentifier}");
Console.WriteLine($"OS: {RuntimeInformation.OSDescription}");

//Extra diagnostics to verify actual target/runtime
var tfm = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
Console.WriteLine($"TFM: {tfm}");
Console.WriteLine($"Environment.Version: {Environment.Version}");

static int Add(int x, int y) => x + y;
