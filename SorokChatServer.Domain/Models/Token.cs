namespace SorokChatServer.Domain.Models;

public record Token(Guid Id, string Email, DateTime CreatedAt, DateTime ExpiresAt);