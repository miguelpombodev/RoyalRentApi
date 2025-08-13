using RoyalRent.Application.DTOs.Outputs;

namespace RoyalRent.Application.Abstractions.Providers;

public interface IAuthenticationProvider
{
    Task<Result<AuthResult>> AuthenticateAsync(string email, string password);
    Task<Result<AuthResult>> RefreshTokenAsync(string refreshToken);
    Task<Result<bool>> RevokeRefreshTokenAsync(string refreshToken);
}
