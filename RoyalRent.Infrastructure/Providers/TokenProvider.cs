using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Application.Providers.Errors;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.ValueObjects;
using StackExchange.Redis;

namespace RoyalRent.Infrastructure.Providers;

public class TokenProvider : ITokenProvider
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly IDatabase _redis;

    private const string RedisRefreshTokenPrefix = "refresh_token:";

    private readonly Jwt _jwt;

    public TokenProvider(IConfiguration configuration, IConnectionMultiplexer redis)
    {
        var jwtConfigurationDict =
            configuration.GetSection("Jwt").GetChildren().ToDictionary(x => x.Key, x => x.Value);

        _redis = redis.GetDatabase();

        _jwt = Jwt.Create(jwtConfigurationDict);
    }

    public string Create(User user)
    {
        var credentials = _buildCredentials();

        var tokenDescriptor = _buildTokenDescriptor(credentials, user);

        var token = _handler.CreateToken(tokenDescriptor);

        return _handler.WriteToken(token);
    }

    public string Decode(string token)
    {
        var splittedToken = token.Split(" ").First(c => c != "Bearer");

        var decodedToken = _handler.ReadToken(splittedToken);

        var tokenInfo = decodedToken as JwtSecurityToken;
        return tokenInfo!.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Email).Value;
    }

    public string CreateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    public async Task<bool> ReplaceRefreshTokenAsync(Guid userId, string newRefreshToken)
    {
        var key = RedisRefreshTokenPrefix.Concat(newRefreshToken).ToString();

        var ttl = TimeSpan.FromDays(7);
        var userData = $"{userId}";

        return await _redis.StringSetAsync(key, userData, ttl);
    }

    public async Task<Result<Guid>> ValidateRefreshTokenAsync(string refreshToken)
    {
        var key = RedisRefreshTokenPrefix.Concat(refreshToken).ToString();
        var value = await _redis.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return Result<Guid>.Failure(AuthenticationErrors.RefreshTokenNotFound);

        return Result<Guid>.Success(Guid.Parse(value!));
    }

    public async Task<bool> DeleteRefreshTokenAsync(string refreshToken)
    {
        var result = await _redis.KeyDeleteAsync(RedisRefreshTokenPrefix.Concat(refreshToken).ToString());

        return result;
    }

    private SigningCredentials _buildCredentials()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));

        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    private SecurityTokenDescriptor _buildTokenDescriptor(SigningCredentials credentials, User user)
    {
        var now = DateTime.UtcNow;

        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ]),
            NotBefore = now.AddSeconds(-30),
            Expires = now.AddMinutes(_jwt.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
        };
    }
}
