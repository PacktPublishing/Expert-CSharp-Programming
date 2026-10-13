// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace WpfMvvmToolkit.Services;

public interface ITimeProvider
{
    DateTime UtcNow { get; }
}

public sealed class UtcTimeProvider : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
