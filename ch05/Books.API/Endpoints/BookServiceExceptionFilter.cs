
using Books.Data;
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

public class ValidateMatchIdEndpointFilter(ILogger<ValidateMatchIdEndpointFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        int id = context.GetArgument<int>(0);
        Book book = context.GetArgument<Book>(1);
        if (id != book.Id)
        {
            logger.LogWarning("Id mismatch with a request");
            return TypedResults.BadRequest();
        }

        return await next(context);
    }
}
