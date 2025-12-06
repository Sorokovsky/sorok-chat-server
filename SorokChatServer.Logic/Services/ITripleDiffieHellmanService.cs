using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Services;

public interface ITripleDiffieHellmanService
{
    public byte[] GenerateSharedKey(DiffieHellmanKeysPair staticKeys, DiffieHellmanKeysPair ephemeralKeys);
}