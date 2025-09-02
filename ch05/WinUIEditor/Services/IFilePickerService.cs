namespace WinUIEditor.Services;

public interface IFilePickerService
{
    Task<string?> PickOpenFileAsync();
    Task<string?> PickSaveFileAsync(string suggestedFileName);
}
