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
        var bytes = MergeBytes(first, second, third);
        return _keyDerivationFunction.GenerateKey(bytes);
    }

    private static byte[] MergeBytes(params byte[][] arrays)
    {
        var result = new byte[arrays.Sum(a => a.Length)];
        var length = 0;
        foreach (var array in arrays)
        {
            Buffer.BlockCopy(array, 0, result, length, array.Length);
            length += array.Length;
        }

        return result;
    }
}