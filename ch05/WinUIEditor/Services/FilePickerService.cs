using Microsoft.UI.Xaml;

using Windows.Storage;
using Windows.Storage.Pickers;

using WinRT.Interop;

namespace WinUIEditor.Services;

public sealed class FilePickerService(Window window) : IFilePickerService
{
    public async Task<StorageFile?> PickOpenFileAsync()
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        FileOpenPicker picker = new();
        InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeFilter.Add("*");
        var file = await picker.PickSingleFileAsync();
        return file;
    }

    public async Task<StorageFile?> PickSaveFileAsync(string suggestedFileName)
    {
        var hwnd = WindowNative.GetWindowHandle(window);
        FileSavePicker picker = new()
        {
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            SuggestedFileName = suggestedFileName
        };
        InitializeWithWindow.Initialize(picker, hwnd);
        picker.FileTypeChoices.Add("Text", [".txt", ".md", ".log", ".cs"]);
        picker.DefaultFileExtension = ".txt";
        var file = await picker.PickSaveFileAsync();
        return file;
    }
}
