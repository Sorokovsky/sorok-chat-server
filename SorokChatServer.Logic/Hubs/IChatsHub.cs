using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Hubs;

public interface IChatsHub
{
    public Task ReceiveMessageAsync(GetMessage message, long chatId);

    public Task ReceiveExchangeAsync(string staticPublicKey, string ephemeralKey, long userId, long chatId);

    public Task ConnectedAsync(long chatId, long userId);
}