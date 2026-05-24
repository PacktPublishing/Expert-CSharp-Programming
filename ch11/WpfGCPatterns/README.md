# WpfGCPatterns — GC Patterns for WPF Applications

> **Platform requirement:** This project targets `net10.0-windows` and **requires Windows** with the .NET 10 SDK to build and run. It cannot be compiled on Linux or macOS.

Demonstrates how the **Garbage Collector interacts with the WPF UI thread** and which patterns prevent frame drops, jank, and memory leaks in desktop applications.

## 🚀 What's Inside

| File | Purpose |
|------|---------|
| `App.xaml` / `App.xaml.cs` | Standard WPF application entry point |
| `MainWindow.xaml` | UI layout with animation, metrics list, and control buttons |
| `MainWindow.xaml.cs` | All GC patterns — latency modes, off-thread work, leak demo |

## 🎓 Skills Demonstrated

### Skill 1 — SustainedLowLatency during animations
WPF targets 60 fps (16 ms per frame). A Gen-2 GC pause of ~50 ms drops multiple frames and causes visible jank.

```csharp
private void OnStartAnimation(object sender, RoutedEventArgs e)
{
    // ✅ Set BEFORE the animation starts
    _previousLatencyMode = GCSettings.LatencyMode;
    GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
    _animationTimer.Start();
}

private void OnStopAnimation(object sender, RoutedEventArgs e)
{
    _animationTimer.Stop();
    // ✅ Restore so idle GC can reclaim memory between user actions
    GCSettings.LatencyMode = _previousLatencyMode;
}
```

**Modes compared:**

| Mode | Gen-2 allowed? | Best for |
|------|---------------|----------|
| `Interactive` | Yes | General WPF, menu interactions |
| `LowLatency` | Avoided | Short animation bursts |
| `SustainedLowLatency` | Avoided long-term | Continuous 60 fps render loops |

### Skill 2 — Off-UI-thread allocations with Task.Run
Heavy data loading on the UI thread can trigger GC during Dispatcher processing, causing the window to freeze.

```csharp
// ❌ Anti-pattern: allocates on UI thread, can stall the Dispatcher
private void LoadData_BAD()
{
    List<byte[]> data = BuildLargeDataset(); // may trigger Gen-2 on UI thread
    DisplayData(data);
}

// ✅ Fix: allocate on thread pool, marshal result back
private async void LoadData_GOOD()
{
    List<byte[]> data = await Task.Run(() => BuildLargeDataset());
    DisplayData(data);  // only the display update runs on UI thread
}
```

### Skill 3 — Event subscription memory leaks
The most common WPF memory leak: a short-lived `Window` subscribes to an event on a long-lived object. The GC cannot collect the window because the event source holds a delegate with a reference to it.

```csharp
// ❌ Anti-pattern: StaticEvent keeps a reference to 'this'
StaticEvent += (_, _) => DoSomething();  // 'this' captured in delegate

// ✅ Fix 1: store the handler and unsubscribe in Closed
private EventHandler? _handler;
private void Subscribe()
{
    _handler = (_, _) => DoSomething();
    StaticEvent += _handler;
}
private void OnClosed(object? s, EventArgs e) => StaticEvent -= _handler;

// ✅ Fix 2: use WeakEventManager (WPF-specific)
WeakEventManager<EventSource, EventArgs>.AddHandler(source, "Event", OnEvent);
```

### Skill 4 — Reduced allocations in DispatcherTimer callbacks
Timer callbacks fire on the UI thread. String interpolation inside a tight timer creates intermediate Gen-0 objects every tick:

```csharp
// ❌ New intermediate strings allocated every 16 ms (60 fps) → 3 600 allocations/min
StatusText.Text = $"Frame {_frame} at {DateTime.Now:HH:mm:ss}";

// ✅ Use string.Create with a stackalloc interpolation handler (.NET 10)
//    Avoids intermediate allocations; the final string is still allocated once.
StatusText.Text = string.Create(null, stackalloc char[64], $"Frame {_frame}");
```

## 🏗️ Project Structure

```
WpfGCPatterns/
├── App.xaml / App.xaml.cs      # Application entry point
├── MainWindow.xaml             # UI layout
├── MainWindow.xaml.cs          # All GC pattern implementations
├── WpfGCPatterns.csproj        # net10.0-windows, UseWPF=true
└── README.md                   # This file
```

## 🔧 GC Configuration for WPF

WPF uses **Workstation GC** by default (one heap, one background GC thread). Server GC is not recommended for UI applications because it allocates more memory per heap than is needed for a single-user desktop app.

```xml
<!-- .csproj — Workstation GC is the correct default for WPF -->
<!-- Do NOT set <ServerGarbageCollection>true</ServerGarbageCollection> -->

<!-- Optional: reduce pauses for animation-heavy apps -->
<!-- Set programmatically instead (see SustainedLowLatency pattern above) -->
```

## ▶️ Building and Running (Windows only)

```bash
cd ch11/WpfGCPatterns
dotnet run
```

The main window shows:
- Live GC metrics refreshed every 500 ms
- **Start/Stop Animation** buttons that toggle `SustainedLowLatency`
- **Background Work** button that offloads allocations to `Task.Run`
- **Event Leak** button to simulate and then fix a static event subscription

## 💡 Key Takeaways

- WPF Workstation GC can pause 10–100 ms for Gen-2 — use `SustainedLowLatency` during animations
- Never allocate large objects on the UI thread — use `Task.Run` + Dispatcher marshal
- Unsubscribe from events in `Closed` / `Dispose` to prevent retention of closed windows
- `string.Create` with `stackalloc` avoids intermediate allocations from interpolation in timer callbacks (the final string is still allocated once)
- `VirtualizingPanel.VirtualizationMode="Recycling"` in ListBox/ListView reuses UI containers — a UI-layer equivalent of object pooling
