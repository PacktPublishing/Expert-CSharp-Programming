using System.Windows;

namespace WPFWithEvents;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

    }

    private void ButtonOne_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Button One Clicked");
    }

    private void ButtonTwo_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Button Two Clicked");
        e.Handled = true;
    }

    private void OuterButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Any button within the StackPanel clicked");
    }
}