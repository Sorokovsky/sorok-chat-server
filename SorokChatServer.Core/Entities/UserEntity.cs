using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Entities;

public class UserEntity : BaseEntity
{
    public Email Email { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string MiddleName { get; set; } = string.Empty;

    public string AvatarPath { get; set; } = string.Empty;
}