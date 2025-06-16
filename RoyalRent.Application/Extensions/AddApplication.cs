using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Application.Accounts;
using RoyalRent.Application.Accounts.Services;
using RoyalRent.Application.Cars.Services;
using RoyalRent.Application.Mappings;

namespace RoyalRent.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddAplicationCollection(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<ICarCommandService, CarCommandService>();
        services.AddScoped<ICarQueryService, CarQueryService>();
        services.AddScoped<IAccountCommandService, AccountCommandService>();
        services.AddScoped<IAccountQueryService, AccountQueryService>();


        return services;
    }
}
