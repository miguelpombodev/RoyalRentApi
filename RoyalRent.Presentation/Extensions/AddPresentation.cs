using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Handlers;
using RoyalRent.Presentation.Mappings;

namespace RoyalRent.Presentation.Extensions;

public static class AddPresentation
{
    public static IServiceCollection AddPresentationCollection(this IServiceCollection services)
    {
        var presentationAssembly = AssemblyReference.Assembly;

        services.AddFluentValidationAutoValidation();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(presentationAssembly);

        services.AddScoped<IAccountHandler, AccountHandler>();

        services.AddScoped<ICookiesHandler, CookiesHandler>();

        services.AddControllers().AddApplicationPart(presentationAssembly);

        return services;
    }
}
