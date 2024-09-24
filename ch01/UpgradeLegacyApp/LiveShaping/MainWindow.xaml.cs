using System.Windows;

namespace LiveShaping;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private LapChart _lapChart = new();
    public MainWindow()
    {
        InitializeComponent();
        DataContext = _lapChart.GetLapInfo();

        Task.Run(async () =>
        {
            bool raceContinues = true;
            while (raceContinues)
            {
                await Task.Delay(3000);
                raceContinues = _lapChart.NextLap();
            }
        });
    }
}

