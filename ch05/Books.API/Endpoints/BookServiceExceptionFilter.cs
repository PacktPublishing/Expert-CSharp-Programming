using Books.Services;

namespace Books.API.Endpoints;

/// <summary>
/// Provides an endpoint filter that handles exceptions of type BookServiceException and logs errors for requests
/// processed by the book service API.
/// </summary>
/// <remarks>This filter should be applied to endpoints that interact with the book service to ensure consistent
/// error handling and logging. When a BookServiceException is thrown during request processing, the filter logs the
/// error and returns a generic internal server error response to the client.</remarks>
/// <param name="logger">The logger used to record error information when a BookServiceException is encountered.</param>
public class BookServiceExceptionFilter(ILogger<BookServiceExceptionFilter> logger) : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            return next(context);
        }
        catch (BookServiceException ex)
        {
            logger.LogError(ex, "Error processing request");
            return ValueTask.FromResult<object?>(TypedResults.InternalServerError());
        }
    }
}
