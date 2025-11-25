using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class TripleDiffieHellmanService : ITripleDiffieHellmanService
{
    private readonly IDiffieHellmanService _diffieHellmanService;
    private readonly IKeyDerivationFunction _keyDerivationFunction;

    public TripleDiffieHellmanService(
        IDiffieHellmanService diffieHellmanService,
        IKeyDerivationFunction keyDerivationFunction
    )
    {
        _diffieHellmanService = diffieHellmanService;
        _keyDerivationFunction = keyDerivationFunction;
    }

    public byte[] GenerateSharedKey(DiffieHellmanKeysPair staticKeys, DiffieHellmanKeysPair ephemeralKeys)
    {
        var firstKey = _diffieHellmanService.GenerateSharedSecret(staticKeys.PrivateKey, ephemeralKeys.PublicKey);
        var secondKey = _diffieHellmanService.GenerateSharedSecret(ephemeralKeys.PrivateKey, staticKeys.PublicKey);
        var thirdKey = _diffieHellmanService.GenerateSharedSecret(ephemeralKeys.PrivateKey, ephemeralKeys.PublicKey);
        var first = firstKey.ToByteArray(true, true);
        var second = secondKey.ToByteArray(true, true);
        var third = thirdKey.ToByteArray(true, true);
        var bytes = new byte[first.Length + second.Length + third.Length];
        Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
        Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
        Buffer.BlockCopy(third, 0, bytes, first.Length + second.Length, third.Length);
        return _keyDerivationFunction.GenerateKey(bytes);
    }
}