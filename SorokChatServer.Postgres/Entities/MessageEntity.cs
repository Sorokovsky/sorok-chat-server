namespace SorokChatServer.Postgres.Entities;

public class MessageEntity : BaseEntity
{
    public string Text { get; set; }

    public string Mac { get; set; }

    public UserEntity Author { get; set; }
}