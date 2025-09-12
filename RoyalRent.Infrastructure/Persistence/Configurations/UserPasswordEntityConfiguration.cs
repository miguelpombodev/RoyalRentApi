using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoyalRent.Domain.Entities;
using RoyalRent.Domain.Users.Entities;
using RoyalRent.Infrastructure.Persistence.Constants;

namespace RoyalRent.Infrastructure.Persistence.Configurations;

public class UserPasswordEntityConfiguration : IEntityTypeConfiguration<UserPassword>
{
    public void Configure(EntityTypeBuilder<UserPassword> builder)
    {
        builder.ToTable(TableNames.UsersPasswords);
        builder.HasKey(password => password.Id);

        builder.Property(password => password.Id).HasColumnName("id").HasColumnType("UUID").IsRequired();
        builder.Property(password => password.PasswordHashed).HasColumnName("password_hashed")
            .HasColumnType("VARCHAR(255)").IsRequired();
        builder.Property(password => password.UserId).HasColumnName("user_id").HasColumnType("UUID").IsRequired();
        builder.Property(password => password.ActualPassword).HasColumnName("actual_password").HasColumnType("BOOLEAN").IsRequired();
        builder.Property(password => password.CreatedOn).HasColumnName("created_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();
        builder.Property(user => user.UpdatedOn).HasColumnName("updated_on")
            .HasColumnType("TIMESTAMP WITH TIME ZONE").IsRequired();

        builder.HasIndex(password => new { password.UserId, password.ActualPassword });

        builder.HasOne<User>(password => password.User).WithMany(user => user.UserPasswords)
            .HasForeignKey(password => password.UserId).HasConstraintName("FK_USER_USER_PASSWORD");
    }
}
