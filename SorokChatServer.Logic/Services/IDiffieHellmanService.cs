using System.Numerics;
using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Services;

public interface IDiffieHellmanService
{
    public DiffieHellmanKeysPair GenerateKeysPair();

    public BigInteger GenerateSharedSecret(BigInteger privateKey, BigInteger otherPublicKey);
}