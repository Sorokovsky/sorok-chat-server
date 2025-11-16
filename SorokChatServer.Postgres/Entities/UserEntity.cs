using SorokChatServer.Logic.Models;

namespace SorokChatServer.Postgres.Entities;

public class UserEntity : BaseEntity
{
    public Name Name { get; set; }

    public Email Email { get; set; }

    public HashedPassword Password { get; set; }

    public string MacSecret { get; set; }
}