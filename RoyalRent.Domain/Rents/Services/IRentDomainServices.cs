using RoyalRent.Domain.Cars.Entities;
using RoyalRent.Domain.Rents.Entities;

namespace RoyalRent.Domain.Rents.Services;

public interface IRentDomainServices
{
    Rent CreateRent(Guid userId, Car car, DateTime startDate, DateTime endDate);
}
