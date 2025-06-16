using Microsoft.AspNetCore.Diagnostics;

namespace RoyalRent.Web.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapErrorHandling(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/api/error", (HttpContext context) =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception is null)
            {
                return Results.Problem();
            }

            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: exception.GetType().Name,
                detail: exception.Message);
        });

        return endpoint;
    }
}
