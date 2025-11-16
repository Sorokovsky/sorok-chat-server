using SorokChatServer.Logic.Entities;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Postgres.Entities;

public class ChatEntity : BaseEntity
{
    public Title Title { get; set; }

    public Description Description { get; set; }

    public List<UserEntity> Members { get; set; }

    public List<MessageEntity> Messages { get; set; }
}