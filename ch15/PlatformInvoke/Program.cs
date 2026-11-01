using System.ComponentModel;

Console.OutputEncoding = System.Text.Encoding.UTF8;

bool showMemory = false;
bool showUptime = false;
bool showHelp = args.Length == 0;

foreach (string argument in args)
{
    switch (argument)
    {
        case "--memory":
            showMemory = true;
            break;
        case "--uptime":
            showUptime = true;
            break;
        case "--all":
            showMemory = true;
            showUptime = true;
            break;
        case "--help":
        case "-h":
            showHelp = true;
            break;
        default:
            Console.Error.WriteLine($"Unknown argument: {argument}");
            PrintUsage(Console.Error);
            return 2;
    }
}

if (showHelp)
{
    PrintUsage(Console.Out);
    return 0;
}

try
{
    SystemInformation info;
    if (OperatingSystem.IsWindows())
    {
        info = WindowsInterop.GetInformation();
    }
    else if (OperatingSystem.IsLinux())
    {
        info = LinuxInterop.GetInformation();
    }
    else
    {
        Console.Error.WriteLine("This sample supports only Windows and Linux.");
        return 1;
    }

    Console.WriteLine("Platform Invoke (P/Invoke) Demo");
    Console.WriteLine("=================================================");
    Console.WriteLine(OperatingSystem.IsWindows() ? "Windows (kernel32.dll)" : "Linux (libc)");
    Console.WriteLine();

    if (showMemory)
    {
        const double bytesPerGib = 1_073_741_824.0;
        Console.WriteLine("Memory Information");
        Console.WriteLine($"   {"Total RAM:",-16}{info.TotalMemoryBytes / bytesPerGib:F2} GiB");
        Console.WriteLine($"   {info.FreeMemoryLabel + ":",-16}{info.FreeMemoryBytes / bytesPerGib:F2} GiB");
        Console.WriteLine();
    }

    if (showUptime)
    {
        TimeSpan uptime = info.Uptime;
        Console.WriteLine($"Uptime: {uptime.Days}d {uptime.Hours:D2}h {uptime.Minutes:D2}m {uptime.Seconds:D2}s");
    }

    return 0;
}
catch (Exception ex) when (ex is Win32Exception or PlatformNotSupportedException
    or DllNotFoundException or EntryPointNotFoundException)
{
    Console.Error.WriteLine($"Unable to query system information: {ex.Message}");
    return 1;
}

static void PrintUsage(TextWriter writer)
{
    writer.WriteLine("Usage: dotnet run -- [--memory] [--uptime] [--all] [--help]");
    writer.WriteLine("  --memory    Show total and available/free physical RAM in GiB.");
    writer.WriteLine("  --uptime    Show elapsed time since system startup.");
    writer.WriteLine("  --all       Show both memory and uptime.");
    writer.WriteLine("  --help, -h  Show this help (also shown when no arguments are supplied).");
}

internal sealed record SystemInformation(
    ulong TotalMemoryBytes, ulong FreeMemoryBytes, string FreeMemoryLabel, TimeSpan Uptime);
