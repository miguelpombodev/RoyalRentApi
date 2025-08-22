using RoyalRent.Domain.Abstractions.Entities;

namespace RoyalRent.Domain.Entities;

public class CarTransmissions : CarBaseEntity, ICarBaseEntity
{
    public CarTransmissions(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
