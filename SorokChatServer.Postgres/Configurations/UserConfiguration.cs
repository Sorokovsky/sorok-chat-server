using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SorokChatServer.Logic.Models;
using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Postgres.Configurations;

public class UserConfiguration : BaseConfiguration<UserEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        builder.OwnsOne(user => user.Email, emailBuilder =>
        {
            emailBuilder.Property(email => email.Value)
                .HasColumnName("email")
                .HasMaxLength(Email.MAX_LENGTH)
                .IsRequired();
            emailBuilder.WithOwner();
        });

        builder.OwnsOne(user => user.Password, passwordBuilder =>
        {
            passwordBuilder.Property(password => password.Value)
                .HasColumnName("password")
                .IsRequired();
            passwordBuilder.WithOwner();
        });
        builder.OwnsOne(user => user.Name, nameBuilder =>
        {
            nameBuilder.Property(name => name.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(Name.MAX_FIRST_NAME_LENGTH)
                .IsRequired();
            nameBuilder.Property(name => name.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(Name.MAX_LAST_NAME_LENGTH)
                .IsRequired();
            nameBuilder.Property(name => name.MiddleName)
                .HasColumnName("middle_name")
                .HasMaxLength(Name.MAX_MIDDLE_NAME_LENGTH)
                .IsRequired();
            nameBuilder.WithOwner();
        });
    }
}