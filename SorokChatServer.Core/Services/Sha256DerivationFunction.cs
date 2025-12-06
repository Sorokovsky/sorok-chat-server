using System.Security.Cryptography;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class Sha256DerivationFunction : IKeyDerivationFunction
{
    private const int KeyLength = 32;

    public byte[] GenerateKey(byte[] seed)
    {
        if (seed.Length == 0)
            throw new ArgumentException("Seed must not be empty", nameof(seed));
        return HKDF.DeriveKey(
            HashAlgorithmName.SHA256,
            seed,
            KeyLength,
            [],
            []
        );
    }
}