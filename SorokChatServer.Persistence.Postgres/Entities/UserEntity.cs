using SorokChatServer.Domain.Models;

namespace SorokChatServer.Persistence.Postgres.Entities;

public class UserEntity : BaseEntity
{
    public UserEntity()
    {
    }

    public UserEntity(
        long id,
        DateTime createdAt,
        DateTime updatedAt,
        Email email,
        HashedPassword hashedPassword,
        FullName fullName
    ) : base(id, createdAt, updatedAt)
    {
        Email = email;
        HashedPassword = hashedPassword;
        FullName = fullName;
    }

    public Email Email { get; set; }
    public HashedPassword HashedPassword { get; set; }
    public FullName FullName { get; set; }
}