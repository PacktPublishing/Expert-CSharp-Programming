// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

namespace AppStartup;

public interface IGreeterService
{
    string Greet(string name);
}

public sealed class GreeterService : IGreeterService
{
    public string Greet(string name) => $"Hello, {name}!";
}
