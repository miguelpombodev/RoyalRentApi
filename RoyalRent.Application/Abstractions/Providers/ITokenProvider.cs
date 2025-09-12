using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Application.Abstractions.Providers;

public interface ITokenProvider
{
    string Create(User user);
    string Decode(string token);
    string CreateRefreshToken();
    Task<bool> ReplaceRefreshTokenAsync(Guid userId, string newRefreshToken);
    Task<Result<Guid>> ValidateRefreshTokenAsync(string refreshToken);
    Task<bool> DeleteRefreshTokenAsync(string refreshToken);

}
