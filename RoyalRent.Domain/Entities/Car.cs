namespace RoyalRent.Domain.Entities;

public class Car : CarBaseEntity
{
    public Car(string name, string model, Guid carMakeId, int year, Guid carTypeId, Guid carColorId,
        string imageUrl, Guid carTransmissionsId, Guid carFuelTypeId, int seats, decimal price, string description) : base(name)
    {
        Model = model;
        CarMakeId = carMakeId;
        Year = year;
        CarTypeId = carTypeId;
        CarColorId = carColorId;
        ImageUrl = imageUrl;
        CarTransmissionsId = carTransmissionsId;
        Seats = seats;
        Price = price;
        CarFuelTypeId = carFuelTypeId;
        Description = description;
    }

    public string Model { get; set; }
    public Guid CarMakeId { get; set; }
    public int Year { get; set; }
    public Guid CarTypeId { get; set; }
    public Guid CarColorId { get; set; }
    public string ImageUrl { get; set; }
    public Guid CarTransmissionsId { get; set; }
    public Guid CarFuelTypeId { get; set; }
    public int Seats { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public CarMake? CarMake { get; set; }
    public CarType? CarType { get; set; }
    public CarColor? CarColor { get; set; }
    public CarTransmissions? CarTransmissions { get; set; }
    public CarFuelType? CarFuelType { get; set; }
}
