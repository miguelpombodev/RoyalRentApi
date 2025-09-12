using Mapster;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Domain.Rents.Services;

namespace RoyalRent.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationCollection(this IServiceCollection services)
    {
        services.AddMapster();
        services.AddScoped<IRentDomainServices, RentDomainServices>();

        return services;
    }
}
