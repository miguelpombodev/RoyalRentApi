using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Providers;

namespace RoyalRent.Presentation.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CookiesHandlerAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var tokenProvider = context.HttpContext.RequestServices.GetRequiredService<ITokenProvider>();

        var accessToken = context.HttpContext.Request.Cookies["access_token"];

        if (string.IsNullOrEmpty(accessToken))
        {
            context.Result = new ContentResult
            {
                Content = "Required cookie was not provided",
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        var decodedToken = tokenProvider.Decode(accessToken!);

        context.HttpContext.Session.Set("session_token", Encoding.ASCII.GetBytes(decodedToken));

        await next();
    }
}
