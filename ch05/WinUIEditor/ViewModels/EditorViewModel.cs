using System.Text;

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
    public partial string Text { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Status { get; set; } = "Ready";

    public string FileName => string.IsNullOrEmpty(FilePath) ? "(new)" : Path.GetFileName(FilePath);

    partial void OnFilePathChanged(string? value) => OnPropertyChanged(nameof(FileName));

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
        try
        {
            var path = await _filePicker.PickOpenFileAsync();
            if (string.IsNullOrEmpty(path))
            {
                Status = "Open canceled";
                return;
            }

            var content = await File.ReadAllTextAsync(path, Encoding.UTF8);
            Text = content;
            FilePath = path;
            Status = $"Opened {FileName}";
        }
        catch (UnauthorizedAccessException ex)
        {
            Status = $"Open denied: {ex.Message}";
        }
        catch (IOException ex)
        {
            Status = $"Open failed: {ex.Message}";
        }
        catch (Exception ex)
        {
            Status = $"Unexpected error opening file: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {   
        try
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                await SaveAsAsync();
                return;
            }

            await File.WriteAllTextAsync(FilePath, Text, Encoding.UTF8);
            Status = $"Saved {FileName}";
        }
        catch (UnauthorizedAccessException ex)
        {
            Status = $"Save denied: {ex.Message}";
        }
        catch (IOException ex)
        {
            Status = $"Save failed: {ex.Message}";
        }
        catch (Exception ex)
        {
            Status = $"Unexpected error saving file: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task SaveAsAsync()
    {
        try
        {
            var suggested = string.IsNullOrEmpty(FileName) ? "document" : Path.GetFileNameWithoutExtension(FileName);
            var path = await _filePicker.PickSaveFileAsync(suggested);
            if (string.IsNullOrEmpty(path))
            {
                Status = "Save canceled";
                return;
            }

            FilePath = path;
            await File.WriteAllTextAsync(FilePath, Text, Encoding.UTF8);
            Status = $"Saved {FileName}";
        }
        catch (UnauthorizedAccessException ex)
        {
            Status = $"Save denied: {ex.Message}";
        }
        catch (IOException ex)
        {
            Status = $"Save failed: {ex.Message}";
        }
        catch (Exception ex)
        {
            Status = $"Unexpected error saving file: {ex.Message}";
        }
    }
}
