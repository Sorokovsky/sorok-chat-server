namespace SorokChatServer.Logic.Contracts;

public record CreateChat(
    long Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Title,
    string Description
);