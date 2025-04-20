using Microsoft.EntityFrameworkCore;
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

    public async Task<User?> GetUserBasicInformationById(Guid id)
    {
        var user = await _context.Set<User>().FirstOrDefaultAsync(user => user.Id == id);

        return user;
    }

    public async Task<User> AddAccount(User user)
    {
        var addedEntry = await _context.AddAsync(user);

        return addedEntry.Entity;
    }

    public Task<User> UpdateAccount(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAccount(User user)
    {
        throw new NotImplementedException();
    }
}
