using Microsoft.AspNetCore.SignalR;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Hubs;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Hubs;

public class ChatsHub : Hub<IChatsHub>
{
    private readonly IChatsService _chatsService;
    private readonly ICurrentUserService _currentUserService;

    public ChatsHub(
        IChatsService chatsService,
        ICurrentUserService currentUserService
    )
    {
        _chatsService = chatsService;
        _currentUserService = currentUserService;
    }

    public override async Task OnConnectedAsync()
    {
        if (_currentUserService.IsAuthenticated is false || _currentUserService.Current is null)
        {
            Context.Abort();
        }
        else
        {
            var chats = await _chatsService.GetByUserAsync(_currentUserService.Current.Id, Context.ConnectionAborted);
            var tasks = chats
                .Select(chat => Task.Run(async () =>
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString(),
                            Context.ConnectionAborted);
                        await Clients.Group(chat.Id.ToString())
                            .ConnectedAsync(chat.Id, _currentUserService.Current.Id);
                    })
                );
            await Task.WhenAll(tasks);
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessageAsync(CreateMessage message, long chatId)
    {
        if (_currentUserService.IsAuthenticated is false || _currentUserService.Current is null)
        {
            Context.Abort();
        }
        else
        {
            var result = Message.Create(message.Text, message.Mac, _currentUserService.Current);
            if (result.IsFailure) return;
            await Clients
                .Group(chatId.ToString())
                .ReceiveMessageAsync(result.Value.ToGet(), chatId);
        }
    }

    public async Task JoinToChatAsync(long chatId)
    {
        if (_currentUserService.IsAuthenticated is false || _currentUserService.Current is null)
        {
            Context.Abort();
        }
        else
        {
            var result = await _chatsService.GetByIdAsync(chatId);
            if (result.IsFailure) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, result.Value.Id.ToString(), Context.ConnectionAborted);
            await Clients.Group(chatId.ToString()).ConnectedAsync(chatId, _currentUserService.Current.Id);
        }
    }

    public async Task SendExchangeAsync(string staticPublicKey, string ephemeralPublicKey, long chatId, long userId)
    {
        await Clients
            .Group(chatId.ToString())
            .ReceiveExchangeAsync(staticPublicKey, ephemeralPublicKey, userId, chatId);
    }
}