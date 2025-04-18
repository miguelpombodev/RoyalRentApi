namespace RoyalRent.Application.DTOs;

public record CreateAccountDto(string Name, string Cpf, string Email, string Telephone, char? Gender);
