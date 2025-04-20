using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;

namespace RoyalRent.Application.Abstractions.Accounts;

public interface IGetUserBasicInformationService
{
    Task<User?> ExecuteGetByIdAsync(Guid id);
}
