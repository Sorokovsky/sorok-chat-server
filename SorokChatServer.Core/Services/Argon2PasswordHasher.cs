using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Options;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class Argon2PasswordHasher : IPasswordHasher
{


    private readonly ArgonOptions _options;

    public Argon2PasswordHasher(IOptions<ArgonOptions> options)
    {
        _options = options.Value;
    }
    
    public async Task<string> HashAsync(string plainPassword, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
        var argon2 = GenerateArgon2Id(Encoding.UTF8.GetBytes(plainPassword), salt);
        var hash = await Task.Run(() => argon2.GetBytes(_options.HashSize), cancellationToken).ConfigureAwait(false);
        var hashBytes = new byte[_options.SaltSize + _options.HashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, _options.SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, _options.SaltSize, _options.HashSize);
        return Convert.ToBase64String(hashBytes);
    }

    public async Task<bool> VerifyAsync(string plainPassword, string hashedPassword, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var hashedBytes = Convert.FromBase64String(hashedPassword);
        if (hashedBytes.Length != _options.SaltSize + _options.HashSize) return false;
        var salt = new byte[_options.SaltSize];
        var expectedHash = new byte[_options.HashSize];
        Buffer.BlockCopy(hashedBytes, 0, salt, 0, _options.SaltSize);
        Buffer.BlockCopy(hashedBytes, _options.SaltSize, expectedHash, 0, _options.HashSize);
        var argon2 = GenerateArgon2Id(Encoding.UTF8.GetBytes(plainPassword), salt);
        var computedHash = await Task.Run(() => argon2.GetBytes(_options.HashSize), cancellationToken).ConfigureAwait(false);
        return CryptographicOperations.FixedTimeEquals(expectedHash, computedHash);
    }

    private Argon2id GenerateArgon2Id(byte[] password, byte[] salt)
    {
        return new Argon2id(password)
        {
            DegreeOfParallelism = _options.DegreeOfParallelism,
            MemorySize = _options.MemorySize,
            Iterations = _options.Iterations,
            Salt = salt,
        };
    }
}