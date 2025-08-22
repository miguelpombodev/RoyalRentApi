using RoyalRent.Domain.Abstractions.Entities;

namespace RoyalRent.Domain.Entities;

public class CarFuelType : CarBaseEntity, ICarBaseEntity
{
    public CarFuelType(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
