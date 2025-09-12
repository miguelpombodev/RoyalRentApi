namespace RoyalRent.Application.DTOs.Outputs;

public record GetUserResponse(string Name, string Cpf, string Email, string Telephone, char? Gender);
