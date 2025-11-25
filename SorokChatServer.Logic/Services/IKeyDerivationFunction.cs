using System.Numerics;

namespace SorokChatServer.Logic.Services;

public interface IKeyDerivationFunction
{
    public string GenerateKey(params BigInteger[] keys);
}