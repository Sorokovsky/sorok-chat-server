using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SorokChatServer.Domain.Models;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres.Configurations;

public class UserEntityConfiguration : BaseEntityConfiguration<UserEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        builder.OwnsOne(user => user.Email, emailBuilder =>
        {
            emailBuilder.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(Email.MaxLength)
                .IsRequired();
            emailBuilder.WithOwner();
        });

        builder.OwnsOne(user => user.HashedPassword, passwordBuilder =>
        {
            passwordBuilder.Property(password => password.Value)
                .HasColumnName("password")
                .HasMaxLength(HashedPassword.MaxLength)
                .IsRequired();
            passwordBuilder.WithOwner();
        });

        builder.OwnsOne(user => user.FullName, fullNameBuilder =>
        {
            fullNameBuilder.Property(fullName => fullName.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(FullName.MaxLength)
                .HasDefaultValue(string.Empty);
            fullNameBuilder.Property(fullName => fullName.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(FullName.MaxLength)
                .HasDefaultValue(string.Empty);
            
            fullNameBuilder.Property(fullName => fullName.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(FullName.MaxLength)
                .HasDefaultValue(string.Empty);
            fullNameBuilder.WithOwner();
        });
    }
}