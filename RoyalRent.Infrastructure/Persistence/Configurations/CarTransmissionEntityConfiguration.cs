using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class CarTransmissionsEntityConfiguration : IEntityTypeConfiguration<CarTransmissions>
{
    public void Configure(EntityTypeBuilder<CarTransmissions> builder)
    {
        builder.ToTable(TableNames.CarTransmissions);
        builder.HasKey(color => color.Id);

        builder.Property(color => color.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(color => color.Name).HasColumnName("name").HasColumnType("VARCHAR(10)").IsRequired();
        builder.Property(color => color.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(color => color.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasIndex(color => new { color.Name });
    }
}
