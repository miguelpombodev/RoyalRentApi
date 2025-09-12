using RoyalRent.Domain.Cars.Entities;
using RoyalRent.Domain.Rents.Entities;

namespace RoyalRent.Domain.Rents.Services;

public class RentDomainServices : IRentDomainServices
{
    public Rent CreateRent(Guid userId, Car car, DateTime startDate, DateTime endDate)
    {
        var rent = new Rent(userId, car.Id, car.Price, startDate,
            endDate);

        return rent;
    }
}
