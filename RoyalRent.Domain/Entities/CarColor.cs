using RoyalRent.Domain.Abstractions.Entities;

namespace RoyalRent.Domain.Entities;

public class CarColor : CarBaseEntity, ICarBaseEntity
{
    public CarColor(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
