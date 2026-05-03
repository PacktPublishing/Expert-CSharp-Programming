using System.Runtime;
using System.Windows;
using System.Windows.Threading;

namespace WpfGCPatterns;

// ============================================================
// MainWindow.xaml.cs — WPF GC Patterns Demo
//
// Key patterns shown:
//
//  1. SustainedLowLatency during animation
//     WPF animates at 60 fps.  A Gen-2 GC pause of ~50 ms
//     drops multiple frames and causes visible jank.
//     → Set LatencyMode=SustainedLowLatency while animating
//       and restore Interactive when the animation is stopped.
//
//  2. Off-UI-thread heavy work
//     Allocating large data structures on the UI thread can
//     trigger GC pauses that block the Dispatcher.
//     → Use Task.Run() to move allocations off the UI thread.
//       Only marshal the final result back via Dispatcher.
//
//  3. Event subscription leak
//     Subscribing to a long-lived object's event from a
//     short-lived window holds a reference, preventing GC.
//     → Always unsubscribe in Closed / Dispose.
//
//  4. DispatcherTimer vs. plain Thread
//     DispatcherTimer fires on the UI thread — keep callbacks
//     allocation-free to avoid triggering GC during a frame.
// ============================================================
public partial class MainWindow : Window
{
    // ── Animation timer ───────────────────────────────────────
    private readonly DispatcherTimer _animationTimer = new() { Interval = TimeSpan.FromMilliseconds(16) };
    private GCLatencyMode _previousLatencyMode;
    private int _frame;

    // ── Metrics refresh timer ─────────────────────────────────
    private readonly DispatcherTimer _metricsTimer = new() { Interval = TimeSpan.FromMilliseconds(500) };

    // ── Leak demo: a static event source that outlives the window ──
    private static event EventHandler? StaticEvent;

    public MainWindow()
    {
        InitializeComponent();
        _animationTimer.Tick += OnAnimationTick;
        _metricsTimer.Tick   += OnMetricsTick;

        // ✅ Unsubscribe when the window closes to prevent a memory leak
        Closed += OnWindowClosed;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        GcModeText.Text  = GCSettings.IsServerGC ? "Server" : "Workstation";
        LatencyText.Text = GCSettings.LatencyMode.ToString();

        _metricsTimer.Start();
        AddMetric("Window loaded — metrics collection started.");
    }

    // ── 1. Animation with SustainedLowLatency ─────────────────

    private void OnStartAnimation(object sender, RoutedEventArgs e)
    {
        // ✅ Switch to SustainedLowLatency BEFORE starting the animation
        //    so there is no Gen-2 collection during the render loop.
        _previousLatencyMode  = GCSettings.LatencyMode;
        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
        LatencyText.Text = GCSettings.LatencyMode.ToString();

        _frame = 0;
        StartAnimationButton.IsEnabled = false;
        StopAnimationButton.IsEnabled  = true;
        _animationTimer.Start();
        AddMetric("▶ Animation started — GC switched to SustainedLowLatency.");
    }

    private void OnStopAnimation(object sender, RoutedEventArgs e)
    {
        _animationTimer.Stop();

        // ✅ Restore the previous latency mode so idle GC can reclaim memory
        GCSettings.LatencyMode = _previousLatencyMode;
        LatencyText.Text = GCSettings.LatencyMode.ToString();

        StartAnimationButton.IsEnabled = true;
        StopAnimationButton.IsEnabled  = false;
        AddMetric($"⏹ Animation stopped at frame {_frame} — GC restored to {_previousLatencyMode}.");
    }

    private void OnAnimationTick(object? sender, EventArgs e)
    {
        _frame++;
        // ✅ string.Create with a stackalloc interpolation handler avoids
        //    intermediate allocations from string interpolation, although it
        //    still allocates the resulting string once per frame tick.
        StatusText.Text = string.Create(null, stackalloc char[32], $"Frame {_frame}");
    }

    // ── 2. Off-UI-thread heavy work ────────────────────────────

    private async void OnHeavyWork(object sender, RoutedEventArgs e)
    {
        HeavyWorkButton.IsEnabled = false;
        AddMetric("⚙ Starting background allocation work…");

        // ✅ Move heavy allocations off the UI thread so the Dispatcher
        //    is never blocked waiting for a GC pause.
        long allocated = await Task.Run(() =>
        {
            long before = GC.GetTotalAllocatedBytes(precise: true);

            // Simulate building a large in-memory dataset
            List<byte[]> chunks = new(1_000);
            for (int i = 0; i < 1_000; i++)
                chunks.Add(new byte[4_096]);

            long after = GC.GetTotalAllocatedBytes(precise: true);
            _ = chunks; // keep alive until after measurement
            return after - before;
        });

        AddMetric($"⚙ Background work done — allocated {allocated / 1024.0:F1} KB off UI thread.");
        HeavyWorkButton.IsEnabled = true;
    }

    // ── 3. Simulate event subscription leak ───────────────────

    private EventHandler? _leakHandler;

    private void OnSimulateLeak(object sender, RoutedEventArgs e)
    {
        if (_leakHandler is null)
        {
            // ❌ Subscribing to a static event without unsubscribing
            //    keeps 'this' window reachable from the GC root even
            //    after the window is closed — a classic WPF memory leak.
            //    The handler references AddMetric, an instance method, so
            //    the delegate holds a strong reference to 'this'.
            _leakHandler = (_, _) => AddMetric("⚠ StaticEvent fired while leak is active.");
            StaticEvent += _leakHandler;
            AddMetric("⚠ Event leak simulated — 'this' is now reachable from a static root.");
            LeakDemoButton.Content = "✅ Fix: Unsubscribe";
        }
        else
        {
            // ✅ Unsubscribing removes the strong reference
            StaticEvent -= _leakHandler;
            _leakHandler = null;
            AddMetric("✅ Unsubscribed — window can now be collected by GC.");
            LeakDemoButton.Content = "⚠ Simulate Event Leak";
        }
    }

    // ── 4. Periodic GC metrics refresh ────────────────────────

    private void OnMetricsTick(object? sender, EventArgs e)
    {
        GCMemoryInfo info  = GC.GetGCMemoryInfo();
        string metric = string.Create(null,
            stackalloc char[128],
            $"[{DateTime.Now:HH:mm:ss.fff}] " +
            $"Heap: {info.HeapSizeBytes / 1024.0:F0} KB | " +
            $"Frag: {info.FragmentedBytes / 1024.0:F0} KB | " +
            $"Gen0: {GC.CollectionCount(0)} | Gen1: {GC.CollectionCount(1)} | Gen2: {GC.CollectionCount(2)}");

        MetricsListBox.Items.Add(metric);

        // Cap the list to avoid unbounded memory growth in the demo
        while (MetricsListBox.Items.Count > 200)
            MetricsListBox.Items.RemoveAt(0);

        MetricsListBox.ScrollIntoView(MetricsListBox.Items[^1]);
    }

    private void AddMetric(string message) =>
        MetricsListBox.Items.Add($">>> {message}");

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        // Restore latency mode if the window was closed while animation was running
        if (_animationTimer.IsEnabled)
        {
            _animationTimer.Stop();
            GCSettings.LatencyMode = _previousLatencyMode;
        }

        _metricsTimer.Stop();

        // ✅ Remove any remaining event subscriptions
        if (_leakHandler is not null)
        {
            StaticEvent -= _leakHandler;
            _leakHandler = null;
        }
    }
}
