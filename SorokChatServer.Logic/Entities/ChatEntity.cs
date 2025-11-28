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
        List<UserEntity> members
    )
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Title = title;
        Description = description;
        Members = members;
    }

    public Title Title { get; set; }

    public Description Description { get; set; }

    public List<UserEntity> Members { get; set; }
}