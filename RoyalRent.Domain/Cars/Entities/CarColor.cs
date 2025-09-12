using RoyalRent.Domain.Abstractions.Entities;
using RoyalRent.Domain.Cars.Entities;

namespace RoyalRent.Domain.Entities;

public class CarColor : CarBaseEntity, ICarBaseEntity
{
    public CarColor(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
