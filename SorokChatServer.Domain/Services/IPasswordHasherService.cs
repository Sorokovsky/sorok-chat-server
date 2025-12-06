namespace SorokChatServer.Domain.Services;

public interface IPasswordHasherService
{
    public string Hash(string plainText);
    public bool Verify(string hashedPassword, string plainText);
}