
using Books.Data;

namespace Books.API.Endpoints;

/// <summary>
/// Provides an endpoint filter that validates whether the route match ID matches the ID of the provided Book object
/// before allowing the request to proceed.
/// </summary>
/// <remarks>Use this filter to ensure that the route parameter and the Book object's ID are consistent, helping
/// to prevent accidental or malicious mismatches in API requests. If the IDs do not match, the filter returns a
/// BadRequest response and does not invoke the next filter or endpoint handler.</remarks>
/// <param name="logger">The logger used to record warnings when an ID mismatch is detected.</param>
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
