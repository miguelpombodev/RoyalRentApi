using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Accounts;
using RoyalRent.Application.Mappings;

namespace RoyalRent.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddAplicationCollection(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<ICreateAccountService, CreateAccountService>();
        services.AddScoped<IGetUserService, GetUserService>();
        services.AddScoped<ICreateDriverLicenseService, CreateDriverLicenseService>();
        services.AddScoped<ILoginService, LoginService>();

        return services;
    }
}
