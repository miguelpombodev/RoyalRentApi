namespace RoyalRent.Domain.Entities;

public class CarType : CarBaseEntity
{
    public CarType(string name) : base(name) { }

    public ICollection<Car>? Cars { get; set; } = new List<Car>();

}
