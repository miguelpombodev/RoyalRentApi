using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;

namespace RoyalRent.Domain.Abstractions;

public interface IAccountRepository
{
    Task<User?> GetUserBasicInformationById(Guid id);
    Task<User?> GetUserByEmail(string email);
    Task<UserPassword?> GetLastActualUserPassword(Guid userId);
    Task<Tuple<User, UserPassword>> AddAccount(User user, UserPassword userPassword);
    Task<User> UpdateAccount(User user);
    Task<User> DeleteAccount(User user);
    Task<UserDriverLicense> AddDriverLicense(UserDriverLicense userDriverLicense);
    Task<UserDriverLicense> UpdateDriverLicense(UserDriverLicense userDriverLicense);
    Task<UserDriverLicense?> GetDriverLicense(Guid id);
    Task<UserPassword> UpdateAccountPassword(Guid id, UserPassword userPassword, Guid lastPasswordId);
}
