using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Cars.Entities;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Rents.Entities;
using RoyalRent.Domain.Users.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class RentsEntityConfiguration : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable(TableNames.Rents);
        builder.HasKey(rent => rent.Id);

        builder.Property(rent => rent.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(rent => rent.UserId).HasColumnName("userId").HasColumnType("UUID").IsRequired();
        builder.Property(rent => rent.CarId).HasColumnName("carId").HasColumnType("UUID").IsRequired();
        builder.Property(rent => rent.RentStartsAt).HasColumnName("rentStartsAt")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(rent => rent.RentFinishesAt).HasColumnName("rentFinishesAt")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(rent => rent.IsPaid).HasColumnName("isPaid").HasColumnType("BOOLEAN").HasDefaultValue(false).IsRequired();
        builder.Property(rent => rent.PaymentAt).HasColumnName("paymentAt").HasColumnType("TIMESTAMP WITH TIME ZONE");
        builder.Property(car => car.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(car => car.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasOne<User>(rent => rent.User).WithMany(user => user.Rents)
            .HasForeignKey(rent => rent.UserId).HasConstraintName("FK_RENT_USER_ID")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Car>(rent => rent.Car).WithMany(car => car.Rents)
            .HasForeignKey(rent => rent.CarId).HasConstraintName("FK_RENT_CAR_ID")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
