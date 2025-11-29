using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Hubs;

public interface IChatsHub
{
    public Task ReceiveMessageAsync(GetMessage message, long chatId);

    public Task ReceiveExchangeAsync(string staticPublicKey, string signingStatic, string ephemeralKey,
        string signingEphemeral, GetUser user, long chatId);

    public Task ConnectedAsync(long chatId, GetUser user);
}