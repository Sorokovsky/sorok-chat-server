namespace SorokChatServer.Domain.Contracts.User;

public record GetUser(
    long Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string MiddleName
);