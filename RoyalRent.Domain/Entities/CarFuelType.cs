namespace RoyalRent.Domain.Entities;

public class CarFuelType : CarBaseEntity
{
    public CarFuelType(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();
}
