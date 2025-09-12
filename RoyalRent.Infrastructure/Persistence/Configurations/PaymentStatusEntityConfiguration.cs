using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Payments.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class PaymentStatusEntityConfiguration : IEntityTypeConfiguration<PaymentStatus>
{
    public void Configure(EntityTypeBuilder<PaymentStatus> builder)
    {
        builder.ToTable(TableNames.PaymentsStatus);
        builder.HasKey(rent => rent.Id);

        builder.Property(rent => rent.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(rent => rent.Name).HasColumnName("name").HasColumnType("VARCHAR(40)").IsRequired();
        builder.Property(rent => rent.StatusColor).HasColumnName("statusColor").HasColumnType("CHAR(7)");
        builder.Property(car => car.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(car => car.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
    }
}
