using Books.Services;

namespace Books.API.Endpoints;

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
