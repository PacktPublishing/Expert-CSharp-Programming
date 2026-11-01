using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
internal static partial class LinuxInterop
{
    // Linux x64/Arm64 use 8-byte C longs. Sequential layout includes native padding.
    [StructLayout(LayoutKind.Sequential)]
    private struct SysInfo
    {
        public long Uptime;
        public ulong Load1;
        public ulong Load5;
        public ulong Load15;
        public ulong TotalRam;
        public ulong FreeRam;
        public ulong SharedRam;
        public ulong BufferRam;
        public ulong TotalSwap;
        public ulong FreeSwap;
        public ushort Procs;
        public ulong TotalHigh;
        public ulong FreeHigh;
        public uint MemUnit;
    }

    [LibraryImport("libc", EntryPoint = "sysinfo", SetLastError = true)]
    private static partial int SysInfoCall(ref SysInfo info);

    public static SystemInformation GetInformation()
    {
        if (RuntimeInformation.ProcessArchitecture is not (Architecture.X64 or Architecture.Arm64))
        {
            throw new PlatformNotSupportedException("The Linux sysinfo layout supports only x64 and Arm64 processes.");
        }

        SysInfo info = default;
        if (SysInfoCall(ref info) != 0)
        {
            int error = Marshal.GetLastPInvokeError();
            throw new Win32Exception(error, $"sysinfo failed with errno {error}.");
        }

        // sysinfo reports memory in units of mem_unit, not necessarily bytes.
        return new SystemInformation(checked(info.TotalRam * info.MemUnit),
            checked(info.FreeRam * info.MemUnit), "Free RAM",
            TimeSpan.FromSeconds(info.Uptime));
    }
}
