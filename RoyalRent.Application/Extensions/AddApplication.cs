using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Accounts;
using RoyalRent.Application.Mappings;

namespace RoyalRent.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection addAplicationCollection(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<ICreateAccountService, CreateAccountService>();

        return services;
    }
}
