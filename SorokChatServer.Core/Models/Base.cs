namespace SorokChatServer.Core.Models;

public abstract class Base
{
    protected Base(long id, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public long Id { get; }
    
    public DateTime CreatedAt { get; }
    
    public DateTime UpdatedAt { get; }
}