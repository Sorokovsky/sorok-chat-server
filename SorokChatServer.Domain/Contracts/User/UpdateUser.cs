namespace SorokChatServer.Domain.Contracts.User;

public record UpdateUser(
    string? Email,
    string? Password,
    string? FirstName,
    string? LastName,
    string? MiddleName
);