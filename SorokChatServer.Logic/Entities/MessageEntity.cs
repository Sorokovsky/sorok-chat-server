using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Logic.Entities;

public class MessageEntity : BaseEntity
{
    public MessageEntity()
    {
    }

    public MessageEntity(long id, DateTime createdAt, DateTime updatedAt, string text, string mac, UserEntity author)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Text = text;
        Mac = mac;
        Author = author;
    }

    public string Text { get; set; }

    public string Mac { get; set; }

    public UserEntity Author { get; set; }
}