using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SorokChatServer.Core.Entities;

namespace SorokChatServer.DataAccess.Configuration;

public class UserConfiguration : BaseConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        base.Configure(builder);
        builder.ComplexProperty(x => x.Email)
            .IsRequired()
            .Property(x => x.Value)
            .HasColumnName("email");
    }
}