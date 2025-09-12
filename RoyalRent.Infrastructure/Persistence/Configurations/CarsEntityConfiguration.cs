using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Cars.Entities;
using RoyalRent.Domain.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class CarsEntityConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable(TableNames.Cars);
        builder.HasKey(car => car.Id);

        builder.Property(car => car.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(car => car.Name).HasColumnName("name").HasColumnType("VARCHAR(40)").IsRequired();
        builder.Property(car => car.Model).HasColumnName("model").HasColumnType("VARCHAR(50)").IsRequired();
        builder.Property(car => car.CarMakeId).HasColumnName("carMakeId").HasColumnType("UUID").IsRequired();
        builder.Property(car => car.Year).HasColumnName("year").HasColumnType("INT").IsRequired();
        builder.Property(car => car.CarTypeId).HasColumnName("carTypeId").HasColumnType("UUID").IsRequired();
        builder.Property(car => car.CarColorId).HasColumnName("carColorId").HasColumnType("UUID").IsRequired();
        builder.Property(car => car.CarTransmissionsId).HasColumnName("carTransmissionId").HasColumnType("UUID")
            .IsRequired();
        builder.Property(car => car.CarFuelTypeId).HasColumnName("carFuelTypeId").HasColumnType("UUID").IsRequired();
        builder.Property(car => car.ImageUrl).HasColumnName("imageUrl").HasColumnType("VARCHAR(255)").IsRequired();
        builder.Property(car => car.Price).HasColumnName("price").HasColumnType("DECIMAL(16,2)").IsRequired();
        builder.Property(car => car.Description).HasColumnName("description").HasColumnType("VARCHAR(3000)")
            .IsRequired();
        builder.Property(car => car.Seats).HasColumnName("seats").HasColumnType("INT").IsRequired();
        builder.Property(car => car.IsFeatured).HasColumnName("isFeatured").HasColumnType("BOOLEAN").IsRequired()
            .HasDefaultValue(false);
        builder.Property(car => car.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(car => car.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasIndex(car => new
        {
            car.Name,
            car.Model,
            car.Year,
            car.CarMakeId,
            car.CarColorId,
            car.CarTypeId,
        });

        builder.HasOne<CarType>(car => car.CarType).WithMany(type => type.Cars)
            .HasForeignKey(car => car.CarTypeId).HasConstraintName("FK_CAR_CAR_TYPE").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CarColor>(car => car.CarColor).WithMany(color => color.Cars)
            .HasForeignKey(car => car.CarColorId).HasConstraintName("FK_CAR_CAR_COLOR")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CarMake>(car => car.CarMake).WithMany(make => make.Cars)
            .HasForeignKey(car => car.CarMakeId).HasConstraintName("FK_CAR_CAR_MAKE").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CarTransmissions>(car => car.CarTransmissions).WithMany(transmission => transmission.Cars)
            .HasForeignKey(car => car.CarTransmissionsId).HasConstraintName("FK_CAR_CAR_TRANSMISSIONS")
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CarFuelType>(car => car.CarFuelType).WithMany(fuelType => fuelType.Cars)
            .HasForeignKey(car => car.CarFuelTypeId).HasConstraintName("FK_CAR_CAR_FUEL_TYPE")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
