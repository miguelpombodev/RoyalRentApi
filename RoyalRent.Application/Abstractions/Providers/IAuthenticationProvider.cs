using RoyalRent.Application.DTOs;
using RoyalRent.Application.DTOs.Outputs;

namespace RoyalRent.Application.Abstractions.Providers;

public interface IAuthenticationProvider
{
    Task<Result<AuthResult>> AuthenticateAsync(LoginDto dto);
    Task<Result<AuthResult>> RefreshTokenAsync(string refreshToken);
    Task<Result<bool>> RevokeRefreshTokenAsync(string refreshToken);
}
