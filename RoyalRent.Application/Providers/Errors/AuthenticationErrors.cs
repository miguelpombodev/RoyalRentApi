using RoyalRent.Application.Abstractions;

namespace RoyalRent.Application.Providers.Errors;

public static class AuthenticationErrors
{
    public static readonly Error RefreshTokenNotFound = new("Authentication.RefreshTokenNotFound", 404,
        "Refresh Token not found! Please be sure!");
    public static readonly Error DeleteRefreshTokenError = new("Authentication.DeleteRefreshTokenError", 409,
        "Refresh Token could not be deleted! Please try again");
}
