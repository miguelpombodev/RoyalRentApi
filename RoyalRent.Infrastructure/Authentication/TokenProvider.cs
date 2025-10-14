using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoyalRent.Application.Abstractions;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Errors;
using RoyalRent.Domain.Users.Entities;
using RoyalRent.Domain.Users.ValueObjects;
using RoyalRent.Domain.ValueObjects;
using StackExchange.Redis;

namespace RoyalRent.Infrastructure.Authentication;

public class TokenProvider : ITokenProvider
{
    private readonly JwtSecurityTokenHandler _handler = new();
    private readonly IDatabase _redis;

    private const string RedisRefreshTokenPrefix = "refresh_token:";
    private readonly TokenValidationParameters _validationParameters;

    private readonly Jwt _jwt;

    public TokenProvider(IConfiguration configuration, IConnectionMultiplexer redis)
    {
        var jwtConfigurationDict =
            configuration.GetSection("Jwt").GetChildren().ToDictionary(x => x.Key, x => x.Value);

        _redis = redis.GetDatabase();

        _jwt = Jwt.Create(jwtConfigurationDict);
        _validationParameters = BuildTokenValidationParameters(jwtConfigurationDict);
    }

    public string Create(User user)
    {
        var credentials = _buildCredentials();

        var tokenDescriptor = BuildTokenDescriptor(credentials, user);

        var token = _handler.CreateToken(tokenDescriptor);

        return _handler.WriteToken(token);
    }

    public async Task<string> Decode(string token)
    {
        var splittedToken = token.Split(" ").First(c => c != "Bearer");

        await _handler.ValidateTokenAsync(splittedToken, _validationParameters);

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
        var key = BuildRedisTokenKey(newRefreshToken);

        var ttl = TimeSpan.FromDays(7);
        var userData = $"{userId}";

        return await _redis.StringSetAsync(key, userData, ttl);
    }

    private string BuildRedisTokenKey(string newRefreshToken)
    {
        return $"{RedisRefreshTokenPrefix}{newRefreshToken}";
    }

    public async Task<Result<Guid>> ValidateRefreshTokenAsync(string refreshToken)
    {
        var key = BuildRedisTokenKey(refreshToken);
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

    private SecurityTokenDescriptor BuildTokenDescriptor(SigningCredentials credentials, User user)
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

    private TokenValidationParameters BuildTokenValidationParameters(Dictionary<string, string?> jwtConfiguration)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfiguration["Jwt:Issuer"],
            ValidAudience = jwtConfiguration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration["Jwt:Secret"]!))
        };
    }
}
