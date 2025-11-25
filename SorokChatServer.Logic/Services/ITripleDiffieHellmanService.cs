using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Services;

public interface ITripleDiffieHellmanService
{
    public string GenerateSharedKey(DiffieHellmanKeysPair staticKeys, DiffieHellmanKeysPair ephemeralKeys);
}