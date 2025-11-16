namespace SorokChatServer.Logic.Contracts;

public record Token(
    Guid Id,
    string Email,
    DateTime CreatedAt,
    DateTime ExpiresAt
);