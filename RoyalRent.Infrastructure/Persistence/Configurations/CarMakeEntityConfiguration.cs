using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class CarMakeEntityConfiguration : IEntityTypeConfiguration<CarMake>
{
    public void Configure(EntityTypeBuilder<CarMake> builder)
    {
        builder.ToTable(TableNames.CarMakes);
        builder.HasKey(make => make.Id);

        builder.Property(make => make.Id).HasColumnName("id").HasColumnType("UUID");
        builder.Property(make => make.Name).HasColumnName("name").HasColumnType("VARCHAR(10)").IsRequired();
        builder.Property(make => make.ImageUrl).HasColumnName("imageUrl").HasColumnType("VARCHAR(255)");
        builder.Property(make => make.CreatedOn).HasColumnName("created_on").IsRequired()
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(make => make.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasIndex(make => new { make.Name });
    }
}
