using SorokChatServer.Logic.Entities;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Postgres.Entities;

public class ChatEntity : BaseEntity
{
    public ChatEntity()
    {
    }

    public ChatEntity(
        long id,
        DateTime createdAt,
        DateTime updatedAt,
        Title title,
        Description description,
        List<UserEntity> members,
        List<MessageEntity> messages
    )
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Title = title;
        Description = description;
        Members = members;
        Messages = messages;
    }

    public Title Title { get; set; }

    public Description Description { get; set; }

    public List<UserEntity> Members { get; set; }

    public List<MessageEntity> Messages { get; set; }
}