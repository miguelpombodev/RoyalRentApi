using RoyalRent.Application.Abstractions;

namespace RoyalRent.Application.Accounts.Errors;

public static class AccountErrors
{
    public static readonly Error UserAccountNotFound = new Error("Account.UserNotFound", 404, "User not found");

    public static readonly Error UserAccountAlreadyRegistered =
        new Error("Account.AlreadyRegistered", 409, "User already registered");

    public static readonly Error UserAccountPasswordDoesNotMatch = new Error("Account.PasswordDoesNotMatch", 404,
        "Informed password does not match with password registered");
    public static readonly Error UserAccountPasswordDoesNotExist = new Error("Account.UserPasswordDoesNotExist", 403, "User password not found");
}
