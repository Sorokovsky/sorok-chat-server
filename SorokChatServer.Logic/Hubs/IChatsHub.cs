using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Hubs;

public interface IChatsHub
{
    public Task<GetMessage> ReceiveMessageAsync(GetMessage message, long chatId);
}