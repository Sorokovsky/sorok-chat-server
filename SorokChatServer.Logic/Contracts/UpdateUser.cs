namespace SorokChatServer.Logic.Contracts;

public record UpdateUser(
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? Email,
    string? Password
);