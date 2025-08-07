namespace RoyalRent.Domain.Entities;

public class CarTransmissions : CarBaseEntity
{
    public CarTransmissions(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
