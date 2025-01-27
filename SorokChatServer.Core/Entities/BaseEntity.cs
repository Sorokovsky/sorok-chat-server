using CSharpFunctionalExtensions;

namespace SorokChatServer.Core.Entities;

public abstract class BaseEntity : Entity<long>
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}