using Books.Services;

namespace BooksService.Extensions;

internal static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Error, Message = "An error occurred while processing the request.")]
    public static partial void LogBookServiceException(this ILogger logger, BookServiceException exception);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Warning, Message = "Id mismatch with a request.")]
    public static partial void LogIdMismatch(this ILogger logger);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Error, Message = "An error occurred while accessing the database.")]
    public static partial void LogDatabaseError(this ILogger logger, Exception exception);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Error, Message = "Error: {exception}")]
    public static partial void LogError(this ILogger logger, Exception exception);


}
