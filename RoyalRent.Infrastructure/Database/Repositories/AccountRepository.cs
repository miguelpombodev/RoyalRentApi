using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Infrastructure.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApiDbContext _context;

    public AccountRepository(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAccount(User user)
    {
        var addedEntry = await _context.AddAsync(user);

        return addedEntry.Entity;
    }
}
