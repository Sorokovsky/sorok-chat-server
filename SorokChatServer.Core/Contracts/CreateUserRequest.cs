using Microsoft.AspNetCore.Http;

namespace SorokChatServer.Core.Contracts;

public record CreateUserRequest(
    string email,
    string password,
    string? surname,
    string? name,
    string? middleName,
    IFormFile? avatar
);