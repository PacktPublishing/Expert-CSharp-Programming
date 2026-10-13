using Microsoft.Extensions.Logging;

using System.Numerics;

namespace GenericsWithDIContainer;
internal class GenericService<T>(ILogger<GenericService<T>> logger)
    where T : INumber<T>
{
    public void WriteGenericType()
    {
        logger.LogInformation(nameof(WriteGenericType));
        Console.WriteLine(typeof(T).Name);
    }
}
