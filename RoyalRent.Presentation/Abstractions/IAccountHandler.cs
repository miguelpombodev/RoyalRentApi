using RoyalRent.Application.DTOs;
using RoyalRent.Domain.Entities;
using RoyalRent.Presentation.Accounts.Requests;

namespace RoyalRent.Presentation.Abstractions;

public interface IAccountHandler
{
    Task SaveAccountAsync(CreateAccountDto request);
    Task<User?> GetUserInformationAsync(Guid id);
}
