using System.Windows;

namespace WpfGCPatterns;

// ============================================================
// App.xaml.cs — WPF application startup
//
// No special GC setup is needed here.  GC tuning is done
// per-operation inside MainWindow, where we know whether the
// UI is in a latency-sensitive animation or an idle state.
// ============================================================
public partial class App : Application { }
