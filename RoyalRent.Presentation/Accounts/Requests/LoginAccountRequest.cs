using RoyalRent.Application.DTOs;

namespace RoyalRent.Presentation.Accounts.Requests;

public record LoginAccountRequest(string Email, string Password) : LoginDto(Email, Password);
