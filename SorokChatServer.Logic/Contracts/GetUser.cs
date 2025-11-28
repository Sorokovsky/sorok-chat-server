namespace SorokChatServer.Logic.Contracts;

public record GetUser(
    long Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Email,
    string FirstName,
    string LastName,
    string MiddleName
);