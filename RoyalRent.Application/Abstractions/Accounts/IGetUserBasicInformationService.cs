using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface IGetUserBasicInformationService
{
    Task<Result<User>> ExecuteGetByIdAsync(Guid id);
}
