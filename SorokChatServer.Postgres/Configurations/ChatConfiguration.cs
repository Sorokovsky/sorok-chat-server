using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SorokChatServer.Logic.Entities;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Postgres.Configurations;

public class ChatConfiguration : BaseConfiguration<ChatEntity>
{
    protected override void ConfigureEntity(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.ToTable("chats");
        builder.OwnsOne(chat => chat.Title, titleBuilder =>
        {
            titleBuilder.Property(title => title.Value)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(Title.MAX_LENGTH);
            titleBuilder.WithOwner();
        });

        builder.OwnsOne(chat => chat.Description, descriptionBuilder =>
        {
            descriptionBuilder.Property(description => description.Value)
                .HasColumnName("description")
                .IsRequired()
                .HasMaxLength(Description.MAX_LENGTH);
            descriptionBuilder.WithOwner();
        });
        builder.HasMany(chat => chat.Members)
            .WithMany();
    }
}