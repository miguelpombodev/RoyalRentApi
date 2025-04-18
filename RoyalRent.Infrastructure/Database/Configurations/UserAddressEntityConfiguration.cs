using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Infrastructure.Database.Constants;

namespace RoyalRent.Infrastructure.Database.Configurations;

public class UserAddressEntityConfiguration : IEntityTypeConfiguration<UserAddress>
{
    public void Configure(EntityTypeBuilder<UserAddress> builder)
    {
        builder.ToTable(TableNames.UsersAddresses);
        builder.HasKey(address => address.Id);

        builder.Property(address => address.Id).HasColumnName("id").HasColumnType("UUID");
        builder.Property(address => address.Cep).HasColumnName("CEP").HasColumnType("CHAR(8)");
        builder.Property(address => address.Address).HasColumnName("address").HasColumnType("VARCHAR")
            .HasMaxLength(250);
        builder.Property(address => address.Number).HasColumnName("number").HasColumnType("VARCHAR").HasMaxLength(5);
        builder.Property(address => address.Neighborhood).HasColumnName("neighborhood").HasColumnType("VARCHAR")
            .HasMaxLength(50);
        builder.Property(address => address.City).HasColumnName("city").HasColumnType("VARCHAR").HasMaxLength(50);
        builder.Property(address => address.FederativeUnit).HasColumnName("UF").HasColumnType("CHAR(2)");
        builder.Property(address => address.UserId).HasColumnName("user_id").HasColumnType("UUID");
        builder.Property(address => address.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(address => address.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasIndex(address => new { address.Cep, address.Address, address.City, address.Id });

        builder.HasOne<User>(address => address.User).WithOne(user => user.UserAddress)
            .HasForeignKey<UserAddress>(address => address.UserId).HasConstraintName("FK_USER_USER_ADDRESSES");
    }
}
