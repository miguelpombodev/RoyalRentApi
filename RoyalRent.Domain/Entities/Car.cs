
namespace RoyalRent.Domain.Entities;

public class Car : CarBaseEntity
{
    public Car(string name, string model, Guid carMakeId, int year, Guid carTypeId, Guid carColorId, string imageUrl) : base(name)
    {
        Model = model;
        CarMakeId = carMakeId;
        Year = year;
        CarTypeId = carTypeId;
        CarColorId = carColorId;
        ImageUrl = imageUrl;
    }

    public string Model { get; set; }
    public Guid CarMakeId { get; set; }
    public int Year { get; set; }
    public Guid CarTypeId { get; set; }
    public Guid CarColorId { get; set; }
    public string ImageUrl { get; set; }
    public CarMake? CarMake { get; set; }
    public CarType? CarType { get; set; }
    public CarColor? CarColor { get; set; }
}
