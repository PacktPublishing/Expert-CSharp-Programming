using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FileUtils.Linux;

[SupportedOSPlatform("Linux")]
internal static partial class LinuxNativeStat
{
    [StructLayout(LayoutKind.Sequential)]
    internal record struct Stat
    {
        public static Stat Empty => default;

        public ulong st_dev;
        public ulong st_ino;
        public ulong st_nlink;
        public uint st_mode;
        public uint st_uid;
        public uint st_gid;
        public int __pad0;
        public ulong st_rdev;
        public long st_size;
        public long st_blksize;
        public long st_blocks;
        public long st_atime;
        public ulong st_atime_nsec;
        public long st_mtime;
        public ulong st_mtime_nsec;
        public long st_ctime;
        public ulong st_ctime_nsec;
        public long __unused;
    }

    // https://www.man7.org/linux/man-pages/man2/stat.2.html

    [LibraryImport("libc", EntryPoint = "stat", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static partial int StateInfo(string path, [MaybeNullWhen(true)] out Stat buf);

    internal static Stat GetFileStat(string path)
    {
        if (StateInfo(path, out Stat stat) != 0)
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());
        }
        if (stat == Stat.Empty)
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());
        }
        return stat;
    }
}
