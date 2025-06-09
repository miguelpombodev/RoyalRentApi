using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace RoyalRent.Presentation.Attributes;

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
