namespace SorokChatServer.Logic;

public abstract class Base
{
    public long Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    protected Base(long id, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}