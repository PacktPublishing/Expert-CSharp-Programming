using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;

namespace WinUIEditor.Services;

public sealed class FilePickerService(Window window) : IFilePickerService
{
    public async Task<string?> PickOpenFileAsync()
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        FileOpenPicker picker = new();
        InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeFilter.Add("*");
        StorageFile file = await picker.PickSingleFileAsync();
        return file?.Path;
    }

    public async Task<string?> PickSaveFileAsync(string suggestedFileName)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        FileSavePicker picker = new();
        InitializeWithWindow.Initialize(picker, hwnd);
        picker.SuggestedFileName = suggestedFileName;
        picker.FileTypeChoices.Add("Text", [".txt", ".md", ".log", ".cs"]);
        picker.DefaultFileExtension = ".txt";
        var file = await picker.PickSaveFileAsync();
        return file?.Path;
    }
}
