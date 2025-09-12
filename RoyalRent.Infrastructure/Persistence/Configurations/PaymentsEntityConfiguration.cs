using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Payments.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class PaymentsEntityConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(TableNames.Payments);
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(payment => payment.UserId).HasColumnName("userId").HasColumnType("UUID").IsRequired();
        builder.Property(payment => payment.PaymentStatusId).HasColumnName("paymentStatusId").HasColumnType("UUID")
            .IsRequired();
        builder.Property(payment => payment.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(payment => payment.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasOne<PaymentStatus>(payment => payment.PaymentStatus).WithMany(paymentStatus => paymentStatus.Payments)
            .HasForeignKey(payment => payment.PaymentStatusId).HasConstraintName("FK_PAYMENT_PAYMENT_STATUS")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
