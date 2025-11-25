using System.Numerics;

namespace SorokChatServer.Logic.Hubs;

public interface IExchangeHub
{
    public Task ReceiveExchangeAsync(BigInteger staticPublicKey, BigInteger ephemeralPublicKey);
}