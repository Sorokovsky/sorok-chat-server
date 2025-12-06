namespace SorokChatServer.Logic.Contracts;

public record GetMessage(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Text,
    string Mac,
    GetUser Author
);