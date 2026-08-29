// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace AppStartup;

public interface IDateTimeProvider
{
    DateTime UtcNow
    {
        get;
    }
}

public sealed class UtcDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
