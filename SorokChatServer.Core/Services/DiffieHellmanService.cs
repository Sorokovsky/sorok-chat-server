using System.Numerics;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Options;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class DiffieHellmanService : IDiffieHellmanService
{
    private readonly BigInteger _g;
    private readonly BigInteger _p;

    public DiffieHellmanService(IOptions<DiffieHellmanOptions> options)
    {
        var opts = options.Value;
        _g = BigInteger.Parse(opts.G);
        _p = new BigInteger(Convert.FromHexString(opts.P), true, true);

        if (_p <= 2)
            throw new InvalidOperationException("P має бути більшим за 2");
        if (_g <= 1 || _g >= _p)
            throw new InvalidOperationException("G має бути в межах [2, P-1)");
        if (BigInteger.ModPow(_g, _p - 1, _p) != 1)
            throw new InvalidOperationException("G не задовольняє g^(p-1) ≡ 1 (mod p) — параметри биті");
    }

    public DiffieHellmanKeysPair GenerateKeysPair()
    {
        var privateKey = GeneratePrivateKey();
        var publicKey = BigInteger.ModPow(_g, privateKey, _p);
        return new DiffieHellmanKeysPair(publicKey, privateKey);
    }

    public BigInteger GenerateSharedSecret(BigInteger privateKey, BigInteger otherPublicKey)
    {
        if (otherPublicKey <= 1 || otherPublicKey >= _p - 1)
            throw new ArgumentOutOfRangeException(nameof(otherPublicKey), "Публічний ключ має бути в [2, P-2]");

        if (otherPublicKey == 1)
            throw new CryptographicException("Отримано тривіальний публічний ключ (1)");

        if (privateKey <= 1 || privateKey >= _p - 1)
            throw new ArgumentOutOfRangeException(nameof(privateKey), "Приватний ключ недійсний");

        return BigInteger.ModPow(otherPublicKey, privateKey, _p);
    }

    private BigInteger GeneratePrivateKey()
    {
        const int extraBits = 64;
        var byteLength = _p.GetByteCount();
        var buffer = new byte[byteLength];
        BigInteger privateKey;
        do
        {
            RandomNumberGenerator.Fill(buffer);
            buffer[^1] &= 0x7F;
            buffer[byteLength - 1] &= 0x7F;

            privateKey = new BigInteger(buffer.AsSpan(0, byteLength), true, true);
            privateKey %= _p - 2;
            privateKey += 2;
        } while (privateKey >= _p - 1);

        return privateKey;
    }
}