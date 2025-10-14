using Microsoft.AspNetCore.Http;

namespace RoyalRent.Presentation.Abstractions;

/// <summary>
/// Provides cookie handling functionality for JWT token extraction and management.
/// Enables secure retrieval of authentication tokens from HTTP request cookies.
/// </summary>
public interface ICookiesHandler
{
    /// <summary>
    /// Extracts the JWT access token from the request cookie collection.
    /// </summary>
    /// <param name="cookies">The HTTP request cookie collection to search for the JWT token</param>
    /// <returns>The JWT token string extracted from the cookies</returns>
    /// <exception cref="ArgumentNullException">Thrown when cookies collection is null</exception>
    /// <exception cref="InvalidOperationException">Thrown when JWT token is not found in cookies</exception>
    Task<string> ExtractJwtTokenFromCookie(IRequestCookieCollection cookies);
}
