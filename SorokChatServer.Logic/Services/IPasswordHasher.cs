namespace SorokChatServer.Logic.Services;

public interface IPasswordHasher
{
    public Task<string> HashAsync(string plainPassword, CancellationToken cancellationToken = default);
    public Task<bool> VerifyAsync(string plainPassword, string hashedPassword, CancellationToken cancellationToken = default);
}