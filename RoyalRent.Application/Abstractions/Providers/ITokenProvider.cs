using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Providers;

public interface ITokenProvider
{
    string Create(User user);
    string Decode(string token);
}
