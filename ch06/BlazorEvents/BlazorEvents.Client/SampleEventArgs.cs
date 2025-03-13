namespace BlazorEvents.Client;

public class SampleEventArgs(string message) : EventArgs
{
    public string Message { get; } = message;
}
