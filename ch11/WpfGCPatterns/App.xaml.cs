using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfGCPatterns;

// ============================================================
// App.xaml.cs — WPF application startup
//
// Registers global handlers that try to notify users when an
// OutOfMemoryException occurs, then exits the process.
// ============================================================
public partial class App : Application
{
    /// <summary>
    /// Registers global exception handlers used by this demo.
    /// </summary>
    /// <param name="e">Startup event data.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    /// <summary>
    /// Unregisters global exception handlers.
    /// </summary>
    /// <param name="e">Exit event data.</param>
    protected override void OnExit(ExitEventArgs e)
    {
        DispatcherUnhandledException -= OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException -= OnCurrentDomainUnhandledException;
        TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;

        base.OnExit(e);
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is OutOfMemoryException oom)
        {
            e.Handled = true;
            HandleOutOfMemory(oom);
        }
    }

    private void OnCurrentDomainUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is OutOfMemoryException oom)
        {
            HandleOutOfMemory(oom);
        }
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Exception? exception = e.Exception.InnerException ?? e.Exception;
        if (exception is OutOfMemoryException oom)
        {
            e.SetObserved();
            HandleOutOfMemory(oom);
        }
    }

    private static void HandleOutOfMemory(OutOfMemoryException exception)
    {
        try
        {
            MessageBox.Show(
                "The application ran out of memory and must close.\n\n" +
                "Try reducing memory pressure or increasing the process/container memory limit.",
                "Out of memory",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (OutOfMemoryException)
        {
            // Avoid further allocations when memory is critically low.
        }

        Environment.FailFast("OutOfMemoryException in WpfGCPatterns.", exception);
    }
}
