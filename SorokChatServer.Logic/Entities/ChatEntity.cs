using SorokChatServer.Logic.Models;
using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Logic.Entities;

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
        List<MessageEntity> messages,
        string staticPublicKey,
        string ephemeralPublicKey
    )
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Title = title;
        Description = description;
        Members = members;
        Messages = messages;
        StaticPublicKey = staticPublicKey;
        EphemeralPublicKey = ephemeralPublicKey;
    }

    public Title Title { get; set; }

    public Description Description { get; set; }

    public List<UserEntity> Members { get; set; }

    public List<MessageEntity> Messages { get; set; }

    public string EphemeralPublicKey { get; set; }

    public string StaticPublicKey { get; set; }
}