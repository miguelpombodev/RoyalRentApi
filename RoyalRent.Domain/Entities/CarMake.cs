namespace RoyalRent.Domain.Entities;

public class CarMake : CarBaseEntity
{
    public CarMake(string name, string? imageUrl = null) : base(name)
    {
        ImageUrl = imageUrl;
    }

    public string? ImageUrl { get; set; }
    public ICollection<Car>? Cars { get; set; } = new List<Car>();


}
