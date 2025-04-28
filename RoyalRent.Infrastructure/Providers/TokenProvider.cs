using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoyalRent.Application.Abstractions.Providers;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.ValueObjects;

namespace RoyalRent.Infrastructure.Providers;

public class TokenProvider : ITokenProvider
{
    private readonly JwtSecurityTokenHandler _handler = new();

    private readonly Jwt _jwt;

    public TokenProvider(IConfiguration configuration)
    {
        var jwtConfigurationDict =
            configuration.GetSection("Jwt").GetChildren().ToDictionary(x => x.Key, x => x.Value);

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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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
