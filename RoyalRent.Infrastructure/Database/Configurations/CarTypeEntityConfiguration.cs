using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Infrastructure.Database.Constants;

namespace RoyalRent.Infrastructure.Database.Configurations;

public class CarTypeEntityConfiguration : IEntityTypeConfiguration<CarType>
{
    public void Configure(EntityTypeBuilder<CarType> builder)
    {
        builder.ToTable(TableNames.CarTypes);
        builder.HasKey(type => type.Id);

        builder.Property(type => type.Id).HasColumnName("id").HasColumnType("UUID");
        builder.Property(type => type.Name).HasColumnName("name").HasColumnType("VARCHAR(40)").IsRequired();
        builder.Property(type => type.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(type => type.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasIndex(type => new { type.Name });
    }
}
