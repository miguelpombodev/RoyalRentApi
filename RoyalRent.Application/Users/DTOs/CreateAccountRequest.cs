namespace RoyalRent.Application.DTOs.Inputs;

public record CreateAccountRequest(string Name, string Cpf, string Email, string Telephone, char? Gender, string Password);
