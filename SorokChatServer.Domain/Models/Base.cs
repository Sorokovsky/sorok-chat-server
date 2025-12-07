namespace SorokChatServer.Domain.Models;

public abstract class Base
{
    public long Id { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public void SetId(long id) => Id = id;
    
    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;

    protected Base(long id, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}