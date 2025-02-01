using Microsoft.AspNetCore.Http;

namespace SorokChatServer.Core.Contracts;

public record UpdateUserRequest(
    string? email,
    string? password,
    string? surname,
    string? name,
    string? middleName,
    IFormFile? avatar
);