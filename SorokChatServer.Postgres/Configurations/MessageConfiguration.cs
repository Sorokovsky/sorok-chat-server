using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Postgres.Configurations;

public class MessageConfiguration : BaseConfiguration<MessageEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.ToTable("messages");
        builder.HasOne(message => message.Author)
            .WithMany();
    }
}