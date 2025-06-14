using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Accounts;
using RoyalRent.Application.Abstractions.Cars;
using RoyalRent.Application.Accounts;
using RoyalRent.Application.Cars.Services;
using RoyalRent.Application.Mappings;

namespace RoyalRent.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddAplicationCollection(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<ICreateAccountService, CreateAccountService>();
        services.AddScoped<IGetUserService, GetUserService>();
        services.AddScoped<IUpdateUserService, UpdateUserService>();
        services.AddScoped<ICreateDriverLicenseService, CreateDriverLicenseService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<ICreateCarService, CreateCarService>();


        return services;
    }
}
