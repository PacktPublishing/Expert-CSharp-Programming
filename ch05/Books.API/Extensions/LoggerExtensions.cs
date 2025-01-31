using Books.Services;

namespace BooksService.Extensions;

public static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1000, Level = LogLevel.Error, Message = "An error occurred while processing the request.")]
    public static partial void LogBookServiceException(this ILogger logger, BookServiceException exception);

}
