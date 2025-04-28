using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Repositories;

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
    Task<UserDriverLicense> GetDriverLicense(Guid id);
}
