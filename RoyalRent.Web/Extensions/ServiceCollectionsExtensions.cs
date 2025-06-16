namespace RoyalRent.Web.Extensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddGlobalErrorException(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            };
        });


        return services;
    }
}
