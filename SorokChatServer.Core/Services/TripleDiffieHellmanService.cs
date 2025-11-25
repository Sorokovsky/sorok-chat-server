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

    public string GenerateSharedKey(DiffieHellmanKeysPair staticKeys, DiffieHellmanKeysPair ephemeralKeys)
    {
        var firstKey = _diffieHellmanService.GenerateSharedSecret(staticKeys.PrivateKey, ephemeralKeys.PublicKey);
        var secondKey = _diffieHellmanService.GenerateSharedSecret(ephemeralKeys.PrivateKey, staticKeys.PublicKey);
        var thirdKey = _diffieHellmanService.GenerateSharedSecret(ephemeralKeys.PrivateKey, ephemeralKeys.PublicKey);
        return _keyDerivationFunction.GenerateKey(firstKey, secondKey, thirdKey);
    }
}