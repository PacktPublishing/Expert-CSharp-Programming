﻿using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace FileUtils.Windows;

[SupportedOSPlatform("Windows")]
internal static partial class WindowsNativeMethods
{
    [LibraryImport("kernel32.dll", EntryPoint = "CreateHardLinkW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateHardLink(
      string newFileName, // [MarshalAs(UnmanagedType.LPWStr)] 
      string existingFileName, // [MarshalAs(UnmanagedType.LPWStr)] 
      nint securityAttributes);

    internal static void CreateHardLink(string oldFileName,
                                        string newFileName)
    {
        if (!CreateHardLink(newFileName, oldFileName, IntPtr.Zero))
        {
            int errorCode = Marshal.GetLastPInvokeError();
            Win32Exception ex = new(errorCode);
            throw new IOException(ex.Message, ex);
        }
    }
}
