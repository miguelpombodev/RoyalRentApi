namespace RoyalRent.Presentation.Accounts.Requests;

public record CreateAccountRequest(string Name, string Cpf, string Email, string Telephone, char? Gender, string password);
