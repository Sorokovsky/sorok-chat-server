using System.Numerics;

namespace SorokChatServer.Logic.Contracts;

public record DiffieHellmanKeysPair(BigInteger PublicKey, BigInteger PrivateKey);