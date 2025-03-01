using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[assembly: DisableRuntimeMarshalling]

namespace FileUtils.Windows;



[SupportedOSPlatform("Windows")]
internal static partial class WindowsNativeFileInfo
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ByHandleFileInformation
    {
        public uint FileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
        public uint VolumeSerialNumber;
        public uint FileSizeHigh;
        public uint FileSizeLow;
        public uint NumberOfLinks;
        public uint FileIndexHigh;
        public uint FileIndexLow;
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetFileInformationByHandle(IntPtr hFile, out ByHandleFileInformation lpFileInformation);

    internal static ByHandleFileInformation GetFileInformation(IntPtr fileHandle)
    {
        if (!GetFileInformationByHandle(fileHandle, out ByHandleFileInformation fileInfo))
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastPInvokeError());
        }
        return fileInfo;
    }
}
