using Microsoft.Extensions.Logging;

namespace Books.Data.Extensions;

internal static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Error, Message = "Error getting books: {Error}")]
    internal static partial void GetBooksError(this ILogger logger, Exception ex, string error);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Error, Message = "Error getting book by id {Id}: {Error}")]
    internal static partial void GetBookByIdError(this ILogger logger, Exception ex, int id, string error);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Error, Message = "Error updating book {Id}: {Error}")]
    internal static partial void UpdateBookError(this ILogger logger, Exception ex, int id, string error);

    [LoggerMessage(EventId = 1004, Level = LogLevel.Error, Message = "Error creating book: {Error}")]
    internal static partial void CreateBookError(this ILogger logger, Exception ex, string error);

    [LoggerMessage(EventId = 1005, Level = LogLevel.Error, Message = "Error deleting book {Id}: {Error}")]
    internal static partial void DeleteBookError(this ILogger logger, Exception ex, int id, string error);

    [LoggerMessage(EventId = 2006, Level = LogLevel.Information, Message = "GetBooks started")]
    internal static partial void GetBooksStart(this ILogger logger);

    [LoggerMessage(EventId = 2007, Level = LogLevel.Information, Message = "Retrieved {Count} books")]
    internal static partial void GetBooksSuccess(this ILogger logger, int count);

    [LoggerMessage(EventId = 2008, Level = LogLevel.Information, Message = "Getting book by id {Id}")]
    internal static partial void GetBookByIdStart(this ILogger logger, int id);

    [LoggerMessage(EventId = 2009, Level = LogLevel.Information, Message = "Creating book '{Title}'")]
    internal static partial void CreateBookStart(this ILogger logger, string title);

    [LoggerMessage(EventId = 2010, Level = LogLevel.Information, Message = "Updating book {Id}")]
    internal static partial void UpdateBookStart(this ILogger logger, int id);

    [LoggerMessage(EventId = 2011, Level = LogLevel.Information, Message = "Updated book {Id} affected {Affected} rows")]
    internal static partial void UpdateBookSuccess(this ILogger logger, int id, int affected);

    [LoggerMessage(EventId = 2012, Level = LogLevel.Information, Message = "Deleting book {Id}")]
    internal static partial void DeleteBookStart(this ILogger logger, int id);

    [LoggerMessage(EventId = 2013, Level = LogLevel.Information, Message = "Created book '{Title}' with id {Id}")]
    internal static partial void CreateBookSuccess(this ILogger logger, string title, int id);

    [LoggerMessage(EventId = 2014, Level = LogLevel.Information, Message = "Deleted book {Id} affected {Affected} rows")]
    internal static partial void DeleteBookSuccess(this ILogger logger, int id, int affected);
}
