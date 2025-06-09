namespace RoyalRent.Presentation.Accounts.Requests;

public record ForgotPasswordRequest(string email, string newPassword);
