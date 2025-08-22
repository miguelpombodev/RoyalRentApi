using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Presentation.Abstractions;
using RoyalRent.Presentation.Attributes;
using RoyalRent.Presentation.Handlers;
using RoyalRent.Presentation.Mappings;

namespace RoyalRent.Presentation.Extensions;

/// <summary>
/// Extension class for configuring presentation layer services in dependency injection.
/// Registers controllers, validation, mapping, and authentication services.
/// </summary>
public static class AddPresentation
{
    /// <summary>
    /// Adds all presentation layer services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to</param>
    /// <returns>The service collection for method chaining</returns>
    /// <remarks>
    /// Configures FluentValidation, AutoMapper, controllers with JSON options, and authentication services.
    /// Sets up circular reference handling and registers custom attributes and handlers.
    /// </remarks>
    public static IServiceCollection AddPresentationCollection(this IServiceCollection services)
    {
        var presentationAssembly = AssemblyReference.Assembly;

        services.AddFluentValidationAutoValidation();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddValidatorsFromAssembly(presentationAssembly);

        services.AddScoped<CookiesHandlerAttribute>();

        services.AddScoped<ICookiesHandler, CookiesHandler>();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddApplicationPart(presentationAssembly);

        return services;
    }
}
