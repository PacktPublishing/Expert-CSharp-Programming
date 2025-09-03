using Windows.Storage;

namespace WinUIEditor.Services;

public interface IFilePickerService
{
    Task<StorageFile?> PickOpenFileAsync();
    Task<StorageFile?> PickSaveFileAsync(string suggestedFileName);
}
