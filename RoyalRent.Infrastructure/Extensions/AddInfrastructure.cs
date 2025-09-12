using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Infrastructure.Authentication;
using RoyalRent.Infrastructure.Cache;
using RoyalRent.Infrastructure.Database;
using RoyalRent.Infrastructure.Hasher;
using StackExchange.Redis;

namespace RoyalRent.Infrastructure.Extensions;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureCollection(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDistribuitedCacheProvider, DistribuitedCacheProvider>();
        services.AddSingleton<IPasswordHasherProvider, PasswordHasherProvider>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();

        var redisConnectionString = configuration.GetConnectionString("RedisConnection")!;

        var redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(redisConnection);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "RoyalRentRedis";
        });

        return services;
    }
}
