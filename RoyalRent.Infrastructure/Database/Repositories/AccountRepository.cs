using Microsoft.EntityFrameworkCore;
using RoyalRent.Application.Repositories;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Infrastructure.Database.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DbSet<User> _userContext;
    private readonly DbSet<UserPassword> _userPasswordContext;
    private readonly DbSet<UserDriverLicense> _userDriverLicenseContext;

    public AccountRepository(ApiDbContext context)
    {
        _userContext = context.Set<User>();
        _userPasswordContext = context.Set<UserPassword>();
        _userDriverLicenseContext = context.Set<UserDriverLicense>();
    }

    public async Task<User?> GetUserBasicInformationById(Guid id)
    {
        var user = await _userContext.FirstOrDefaultAsync(user => user.Id == id);

        return user;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _userContext.FirstOrDefaultAsync(user => user.Email == email);

        return user;
    }

    public async Task<UserPassword?> GetLastActualUserPassword(Guid userId)
    {
        var userLastActualPassword = await _userPasswordContext.FirstOrDefaultAsync(userPassword => userPassword.UserId == userId && userPassword.ActualPassword);

        return userLastActualPassword;
    }

    public async Task<Tuple<User, UserPassword>> AddAccount(User user, UserPassword userPassword)
    {
        var addedEntry = await _userContext.AddAsync(user);
        var addedUserPasswordEntry = await _userPasswordContext.AddAsync(userPassword);

        return Tuple.Create(addedEntry.Entity, addedUserPasswordEntry.Entity);
    }

    public Task<User> UpdateAccount(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAccount(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDriverLicense> AddDriverLicense(UserDriverLicense userDriverLicense)
    {
        var addedEntry = await _userDriverLicenseContext.AddAsync(userDriverLicense);

        return addedEntry.Entity;
    }

    public Task<UserDriverLicense> UpdateDriverLicense(UserDriverLicense userDriverLicense)
    {
        throw new NotImplementedException();
    }

    public Task<UserDriverLicense> GetDriverLicense(Guid id)
    {
        throw new NotImplementedException();
    }
}
