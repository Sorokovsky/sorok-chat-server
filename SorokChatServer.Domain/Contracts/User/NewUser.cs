namespace SorokChatServer.Domain.Contracts.User;

public record NewUser(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string MiddleName
);