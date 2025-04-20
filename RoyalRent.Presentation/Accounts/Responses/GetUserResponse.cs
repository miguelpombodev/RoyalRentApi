namespace RoyalRent.Presentation.Accounts.Responses;

public record GetUserResponse(string Name, string Cpf, string Email, string Telephone, char? Gender);
