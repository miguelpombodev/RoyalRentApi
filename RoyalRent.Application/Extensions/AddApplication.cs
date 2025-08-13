using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalRent.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationCollection(this IServiceCollection services)
    {
        services.AddMapster();

        return services;
    }
}
