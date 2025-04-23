using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Infrastructure.Abstractions;
using RoyalRent.Infrastructure.Providers;

namespace RoyalRent.Infrastructure.Extensions;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureCollection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDistribuitedCacheService, DistribuitedCacheService>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "RoyalRentRedis";
        });

        return services;
    }
}
