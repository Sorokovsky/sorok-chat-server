namespace SorokChatServer.Logic.Contracts;

public record GetChat(
    long Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Title,
    string Description,
    List<GetUser> Members
);