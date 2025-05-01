using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface IGetUserService
{
    Task<Result<User>> ExecuteGetByIdAsync(Guid id);
    Task<Result<User>> ExecuteGetByEmailAsync(string email);
}
