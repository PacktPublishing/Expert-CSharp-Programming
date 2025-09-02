using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WinUIEditor.Services;

namespace WinUIEditor.ViewModels;

public partial class EditorViewModel(IFilePickerService filePicker) : ObservableObject
{
    private readonly IFilePickerService _filePicker = filePicker;

    [ObservableProperty]
    public partial string? FilePath { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; }

    [ObservableProperty]
    public partial string Status { get; set; } = "Ready";

    public string FileName => string.IsNullOrEmpty(FilePath) ? "(new)" : Path.GetFileName(FilePath);

    partial void OnFilePathChanged(string? value)
    {
        OnPropertyChanged(nameof(FileName));
    }

    [RelayCommand]
    private void New()
    {
        Text = string.Empty;
        FilePath = null;
        Status = "New document";
    }

    [RelayCommand]
    private async Task OpenAsync()
    {
        var path = await _filePicker.PickOpenFileAsync();
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        Text = await File.ReadAllTextAsync(path, Encoding.UTF8);
        FilePath = path;
        Status = "Opened";
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrEmpty(FilePath))
        {
            await SaveAsAsync();
            return;
        }

        await File.WriteAllTextAsync(FilePath, Text, Encoding.UTF8);
        Status = "Saved";
    }

    [RelayCommand]
    private async Task SaveAsAsync()
    {
        var suggested = string.IsNullOrEmpty(FileName) ? "document" : Path.GetFileNameWithoutExtension(FileName);
        var path = await _filePicker.PickSaveFileAsync(suggested);
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        FilePath = path;
        await File.WriteAllTextAsync(FilePath, Text, Encoding.UTF8);
        Status = "Saved";
    }
}
