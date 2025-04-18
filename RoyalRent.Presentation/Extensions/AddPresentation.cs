using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Presentation.Mappings;

namespace RoyalRent.Presentation.Extensions;

public static class AddPresentation
{
    public static IServiceCollection addPresentationCollection(this IServiceCollection services)
    {
        var presentationAssembly = AssemblyReference.Assembly;

        services.AddFluentValidationAutoValidation();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(presentationAssembly);

        services.AddControllers().AddApplicationPart(presentationAssembly);

        return services;
    }
}
