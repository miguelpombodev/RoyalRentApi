namespace RoyalRent.Application.DTOs.Inputs;

public record ForgotPasswordRequest(string Email, string NewPassword);
