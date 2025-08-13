using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RoyalRent.Application.Abstractions.Providers;

namespace RoyalRent.Presentation.Attributes;

/// <summary>
/// Action filter attribute that handles JWT token extraction from cookies and session management.
/// Validates access tokens and stores decoded user information in session for authenticated endpoints.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CookiesHandlerAttribute : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Executes the action filter to validate JWT tokens and manage user sessions.
    /// </summary>
    /// <param name="context">The action executing context containing request information</param>
    /// <param name="next">The next action in the pipeline</param>
    /// <returns>Task representing the asynchronous operation</returns>
    /// <remarks>
    /// Extracts JWT token from cookies, validates it, and stores user info in session.
    /// Returns 401 Unauthorized if access token is missing.
    /// </remarks>
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
