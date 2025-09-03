using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

using WinRT.Interop;

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
            StorageFile? file = await _filePicker.PickOpenFileAsync();
            if (file is null)
            {
                Status = "Open canceled";
                return;
            }

            FilePath = file.Path;
            IRandomAccessStreamWithContentType stream = await file.OpenReadAsync();
            using DataReader reader = new(stream);
            await reader.LoadAsync((uint)stream.Size);
            Text = reader.ReadString((uint)stream.Size);

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
        if (FilePath is null)
        {
            Status = "No file to save";
            return;
        }
        
        StorageFile file = await StorageFile.GetFileFromPathAsync(FilePath);
        await FileIO.WriteTextAsync(file, Text, Windows.Storage.Streams.UnicodeEncoding.Utf8);

        Status = $"Saved {FileName}";
    }

    [RelayCommand]
    private async Task SaveAsAsync()
    {
        try
        {
            var suggested = string.IsNullOrEmpty(FileName) ? "New Document" : Path.GetFileNameWithoutExtension(FileName);
            var file = await _filePicker.PickSaveFileAsync(suggested);
            if (file is null)
            {
                Status = "Save canceled";
                return;
            }

            FilePath = file.Path;

            using StorageStreamTransaction tx = await file.OpenTransactedWriteAsync();
            IRandomAccessStream stream = tx.Stream;
            stream.Seek(0);
            using DataWriter writer = new(stream);
            writer.WriteString(Text);
            tx.Stream.Size = await writer.StoreAsync();
            await tx.CommitAsync();
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
