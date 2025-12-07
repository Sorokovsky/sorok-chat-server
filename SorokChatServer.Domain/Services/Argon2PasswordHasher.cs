using Isopoh.Cryptography.Argon2;

namespace SorokChatServer.Domain.Services;

public class Argon2PasswordHasher : IPasswordHasherService
{

    public string Hash(string plainText)
    {
        return Argon2.Hash(plainText);
    }

    public bool Verify(string hashedPassword, string plainText)
    {
        return Argon2.Verify(hashedPassword, plainText);
    }
}