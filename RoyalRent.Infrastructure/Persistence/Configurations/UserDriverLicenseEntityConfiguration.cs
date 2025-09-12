using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class UserDriverLicenseEntityConfiguration : IEntityTypeConfiguration<UserDriverLicense>
{
    public void Configure(EntityTypeBuilder<UserDriverLicense> builder)
    {
        builder.ToTable(TableNames.UsersDriverLicenses);
        builder.HasKey(license => license.Id);

        builder.Property(license => license.Id).HasColumnName("id").HasColumnType("UUID");
        builder.Property(license => license.Rg).HasColumnName("RG").HasColumnType("CHAR(9)");
        builder.Property(license => license.BirthDate).HasColumnName("birthdate").HasColumnType("DATE");
        builder.Property(license => license.DriverLicenseNumber).HasColumnName("CNH").HasColumnType("CHAR(11)");
        builder.Property(license => license.DocumentExpirationDate).HasColumnName("document_expiration_date")
            .HasColumnType("DATE");
        builder.Property(license => license.State).HasColumnName("state").HasColumnType("CHAR(2)");
        builder.Property(license => license.UserId).HasColumnName("user_id").HasColumnType("UUID");
        builder.Property(license => license.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(license => license.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasOne<User>(license => license.User).WithOne(user => user.UserDriverLicense)
            .HasForeignKey<UserDriverLicense>(license => license.UserId).HasConstraintName("FK_USER_USER_LICENSE");
    }
}
