namespace SorokChatServer.Logic.Contracts;

public record CreateUser(
    string FirstName,
    string LastName,
    string MiddleName,
    string Email,
    string Password
);