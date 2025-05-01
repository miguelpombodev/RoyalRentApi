using Microsoft.AspNetCore.Http;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Presentation.Abstractions;

namespace RoyalRent.Presentation.Handlers;

public class CookiesHandler : ICookiesHandler
{
    private readonly ITokenProvider _tokenProvider;

    public CookiesHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public string ExtractJwtTokenFromCookie(IRequestCookieCollection cookies)
    {
        var access_token = cookies["access_token"]!;

        var decodedToken = _tokenProvider.Decode(access_token);

        return decodedToken;
    }
}
