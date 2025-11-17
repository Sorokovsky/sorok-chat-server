namespace SorokChatServer.Logic.Contracts;

public record GetMessage(
    long Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Text,
    string Mac,
    GetUser Author
);