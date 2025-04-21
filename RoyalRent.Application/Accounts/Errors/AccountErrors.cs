using RoyalRent.Application.Abstractions;

namespace RoyalRent.Application.Accounts.Errors;

public static class AccountErrors
{
    public static readonly Error UserAccountNotFound = new Error("Account.UserNotFound", 404, "User not found");
}
