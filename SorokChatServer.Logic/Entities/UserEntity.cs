using SorokChatServer.Logic.Models;
using SorokChatServer.Postgres.Entities;

namespace SorokChatServer.Logic.Entities;

public class UserEntity : BaseEntity
{
    public UserEntity()
    {
    }

    public UserEntity(long id, DateTime createdAt, DateTime updatedAt, Name name, Email email, HashedPassword password,
        string publicRsaKey)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Name = name;
        Email = email;
        Password = password;
        PublicRsaKey = publicRsaKey;
    }

    public Name Name { get; set; }

    public Email Email { get; set; }

    public HashedPassword Password { get; set; }

    public string PublicRsaKey { get; set; }
}