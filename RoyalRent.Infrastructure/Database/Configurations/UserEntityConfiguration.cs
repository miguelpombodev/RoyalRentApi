using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Infrastructure.Database.Constants;

namespace RoyalRent.Infrastructure.Database.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableNames.Users);
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(user => user.Name).HasColumnName("name").HasColumnType("VARCHAR").HasMaxLength(650).IsRequired();
        builder.Property(user => user.Cpf).HasColumnName("CPF").HasColumnType("CHAR(11)").IsRequired();
        builder.Property(user => user.Email).HasColumnName("email").HasColumnType("VARCHAR").HasMaxLength(150).IsRequired();
        builder.Property(user => user.Gender).HasColumnName("gender").HasColumnType("CHAR(1)").IsRequired();
        builder.Property(user => user.Telephone).HasColumnName("phone").HasColumnType("CHAR(12)").IsRequired();
        builder.Property(user => user.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").ValueGeneratedOnAdd();
        builder.Property(user => user.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").ValueGeneratedOnAddOrUpdate();

        builder.HasIndex(user => new { user.Email, user.Cpf, user.Telephone });
    }
}