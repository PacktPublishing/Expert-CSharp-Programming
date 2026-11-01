using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
internal static partial class WindowsInterop
{
    [StructLayout(LayoutKind.Sequential)]
    private struct MemoryStatusEx
    {
        public uint Length;
        public uint MemoryLoad;
        public ulong TotalPhys;
        public ulong AvailPhys;
        public ulong TotalPageFile;
        public ulong AvailPageFile;
        public ulong TotalVirtual;
        public ulong AvailVirtual;
        public ulong AvailExtendedVirtual;
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GlobalMemoryStatusEx(ref MemoryStatusEx buffer);

    [LibraryImport("kernel32.dll")]
    private static partial ulong GetTickCount64();

    public static SystemInformation GetInformation()
    {
        MemoryStatusEx memory = new()
        {
            // Win32 requires the size of MEMORYSTATUSEX before filling the buffer.
            Length = (uint)Marshal.SizeOf<MemoryStatusEx>()
        };

        if (!GlobalMemoryStatusEx(ref memory))
        {
            int error = Marshal.GetLastPInvokeError();
            throw new Win32Exception(error, $"GlobalMemoryStatusEx failed with error {error}.");
        }

        return new SystemInformation(memory.TotalPhys, memory.AvailPhys,
            "Available RAM", TimeSpan.FromMilliseconds(GetTickCount64()));
    }
}
