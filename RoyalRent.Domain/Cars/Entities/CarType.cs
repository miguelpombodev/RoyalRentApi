using RoyalRent.Domain.Abstractions.Entities;
using RoyalRent.Domain.Cars.Entities;

namespace RoyalRent.Domain.Entities;

public class CarType : CarBaseEntity, ICarBaseEntity
{
    public CarType(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();

}
