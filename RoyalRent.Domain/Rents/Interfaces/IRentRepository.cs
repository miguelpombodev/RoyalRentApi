using RoyalRent.Domain.Rents.Entities;

namespace RoyalRent.Domain.Rents.Interfaces;

public interface IRentRepository
{
    Task<Rent> Create(Rent rent);
}
