using FileUtils.Linux;
using FileUtils.Windows;

using System.IO;

namespace FileUtils;

public static class FileUtility
{
    public static void CreateHardLink(string oldFileName,
                                      string newFileName)
    {
        if (OperatingSystem.IsWindows())
        {
            WindowsNativeMethods.CreateHardLink(oldFileName, newFileName);
        }
        else if (OperatingSystem.IsLinux())
        {
            LinuxNativeMethods.CreateHardLink(oldFileName, newFileName);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

//    string filePath = "example.txt";

//        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
//        {
//            var stat = FileUtils.Linux.LinuxNativeMethods.GetFileStat(filePath);
//    Console.WriteLine($"File size: {stat.st_size}");
//        }
//        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
//        {
//            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
//            {
//                var fileInfo = FileUtils.Windows.WindowsNativeMethods.GetFileInformation(fileStream.SafeFileHandle.DangerousGetHandle());
//Console.WriteLine($"File size: {((long)fileInfo.FileSizeHigh << 32) + fileInfo.FileSizeLow}");
//            }
//        }
}