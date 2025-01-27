namespace SorokChatServer.Core.Interfaces;

public interface IPasswordService
{
    public Task<string> Encrypt(string password);

    public Task<bool> IsEqual(string rawPassword, string hashedPassword);
}