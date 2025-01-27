using DevOne.Security.Cryptography.BCrypt;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Options;

namespace SorokChatServer.Core.Services;

public class PasswordService : IPasswordService
{
    private readonly IOptionsMonitor<HashingOptions> _options;

    public PasswordService(IOptionsMonitor<HashingOptions> options)
    {
        _options = options;
    }

    public Task<string> Encrypt(string password)
    {
        var salt = BCryptHelper.GenerateSalt(_options.CurrentValue.Rounds) ?? string.Empty;
        return Task.FromResult(BCryptHelper.HashPassword(password, salt));
    }

    public Task<bool> IsEqual(string rawPassword, string hashedPassword)
    {
        return Task.FromResult(BCryptHelper.CheckPassword(rawPassword, hashedPassword));
    }
}