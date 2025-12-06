namespace SorokChatServer.Logic.Services;

public interface IKeyDerivationFunction
{
    public byte[] GenerateKey(byte[] seed);
}