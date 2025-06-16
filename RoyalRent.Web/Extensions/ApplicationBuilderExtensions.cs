using Microsoft.AspNetCore.Diagnostics;

namespace RoyalRent.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler("/api/error");

        return app;
    }
}
