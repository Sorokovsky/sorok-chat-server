using System.Numerics;

namespace SorokChatServer.Logic.Hubs;

public interface IExchangeHub
{
    public Task ReceiveExchange(BigInteger staticPublicKey, BigInteger ephemeralPublicKey);
}