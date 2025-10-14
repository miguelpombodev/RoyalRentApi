using Microsoft.AspNetCore.Http;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Presentation.Abstractions;

namespace RoyalRent.Presentation.Handlers;

/// <summary>
/// Implementation of ICookiesHandler for JWT token extraction from HTTP cookies.
/// Provides secure token retrieval and decoding functionality.
/// </summary>
public class CookiesHandler : ICookiesHandler
{
    private readonly ITokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of the CookiesHandler class.
    /// </summary>
    /// <param name="tokenProvider">Service for JWT token decoding operations</param>
    public CookiesHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    /// <summary>
    /// Extracts and decodes the JWT access token from request cookies.
    /// </summary>
    /// <param name="cookies">The HTTP request cookie collection</param>
    /// <returns>The decoded JWT token content</returns>
    /// <remarks>
    /// Retrieves the "access_token" cookie and decodes it using the token provider.
    /// Assumes the access token exists in the cookie collection.
    /// </remarks>
    public async Task<string> ExtractJwtTokenFromCookie(IRequestCookieCollection cookies)
    {
        var access_token = cookies["access_token"]!;

        var decodedToken = await _tokenProvider.Decode(access_token);

        return decodedToken;
    }
}
