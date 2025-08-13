using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RoyalRent.Presentation.Attributes;

/// <summary>
/// Action filter attribute that implements Redis caching for controller methods.
/// Automatically caches successful responses and serves cached data when available.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class RedisCacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _cacheKey;
    private readonly DistributedCacheEntryOptions _cacheEntryOptions;
    private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    /// <summary>
    /// Initializes a new instance of the RedisCacheAttribute with the specified cache key.
    /// </summary>
    /// <param name="cacheKey">The unique cache key for storing and retrieving cached data</param>
    /// <exception cref="ArgumentException">Thrown when cache key is null or empty</exception>
    public RedisCacheAttribute(string cacheKey)
    {
        if (string.IsNullOrEmpty(cacheKey))
            throw new ArgumentException("Cache Key is needed to use method");

        _cacheKey = cacheKey;
        _cacheEntryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
        };
    }

    /// <summary>
    /// Executes caching logic before and after action execution.
    /// Checks for cached data first, then caches successful responses.
    /// </summary>
    /// <param name="context">The action executing context</param>
    /// <param name="next">The next action in the pipeline</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <remarks>
    /// Returns cached data if available, otherwise executes action and caches successful results.
    /// Uses 60-second cache expiration and camelCase JSON serialization.
    /// </remarks>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
        var cachedResponse = await cache.GetStringAsync(_cacheKey);

        if (!string.IsNullOrEmpty(cachedResponse))
        {
            context.Result = new ContentResult { Content = cachedResponse, StatusCode = 200 };

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult && objectResult.Value is not null)
        {
            var serializedData = JsonConvert.SerializeObject(objectResult.Value, Formatting.Indented, jsonSerializerSettings);
            await cache.SetStringAsync(_cacheKey, serializedData, _cacheEntryOptions);
        }
    }
}
