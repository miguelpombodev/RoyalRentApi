using Microsoft.AspNetCore.Http;

namespace RoyalRent.Presentation.Abstractions;

public interface ICookiesHandler
{
    string ExtractJwtTokenFromCookie(IRequestCookieCollection cookies);
}
