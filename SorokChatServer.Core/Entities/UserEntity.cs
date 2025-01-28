using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Entities;

public class UserEntity : BaseEntity
{
    public Email Email { get; set; }

    public string Password { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    public string MiddleName { get; set; }

    public string AvatarPath { get; set; }
}