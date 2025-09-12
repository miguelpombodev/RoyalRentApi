namespace RoyalRent.Application.Rents.DTOs;

public record CreateRentRequest(
        Guid CarId,
        DateTime StartDate,
        DateTime EndDate,
        Guid UserId);
