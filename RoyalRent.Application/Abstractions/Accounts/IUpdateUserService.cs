using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface IUpdateUserService
{
    Task<Result<string>> UpdateUserPassword(Guid id, string newPassword);
}
