namespace SorokChatServer.Core.Contracts;

public record UserResponse(
    long id,
    DateTime createdAt,
    DateTime updatedAt,
    string email,
    string surname,
    string name,
    string middleName,
    string avatarPath
);