using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Repositories;

public interface IAccountRepository
{
    Task<User> AddAccount(User user);
}
