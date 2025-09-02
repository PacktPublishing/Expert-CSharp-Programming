using Microsoft.UI.Xaml;

using WinUIEditor.Services;
using WinUIEditor.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIEditor;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public EditorViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        var filePicker = new FilePickerService(this);
        ViewModel = new EditorViewModel(filePicker);
        // Optional: still set DataContext if any classic {Binding} remains
        Root.DataContext = ViewModel;
    }
}
