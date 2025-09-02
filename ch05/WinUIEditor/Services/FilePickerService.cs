using Microsoft.UI.Xaml;

using Windows.Storage.Pickers;

using WinRT.Interop;

namespace WinUIEditor.Services;

public sealed class FilePickerService(Window window) : IFilePickerService
{
    public async Task<string?> PickOpenFileAsync()
    {
        try
        {
            var hwnd = WindowNative.GetWindowHandle(window);
            var picker = new FileOpenPicker();
            InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Add("*");
            var file = await picker.PickSingleFileAsync();
            return file?.Path;
        }
        catch
        {
            // Swallow and let ViewModel set a friendly status message.
            return null;
        }
    }

    public async Task<string?> PickSaveFileAsync(string suggestedFileName)
    {
        try
        {
            var hwnd = WindowNative.GetWindowHandle(window);
            var picker = new FileSavePicker();
            InitializeWithWindow.Initialize(picker, hwnd);
            picker.SuggestedFileName = suggestedFileName;
            picker.FileTypeChoices.Add("Text", [".txt", ".md", ".log", ".cs"]);
            picker.DefaultFileExtension = ".txt";
            var file = await picker.PickSaveFileAsync();
            return file?.Path;
        }
        catch
        {
            // Swallow and let ViewModel set a friendly status message.
            return null;
        }
    }
}
