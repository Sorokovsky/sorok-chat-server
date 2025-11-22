using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Hubs;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Hubs;

public class ChatsHub : PrivateHub<IChatsHub>
{
    private readonly IChatsService _chatsService;
    private readonly ICurrentUserService _currentUserService;

    public ChatsHub(
        IUsersService usersService,
        ITokenSerializerService tokenSerializerService,
        IChatsService chatsService,
        ICurrentUserService currentUserService
    ) : base(usersService, tokenSerializerService)
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
                .Select(chat =>
                    Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString(), Context.ConnectionAborted)
                );
            await Task.WhenAll(tasks);
        }

        await base.OnConnectedAsync();
    }

    public async Task SendMessage(CreateMessage message, long chatId)
    {
        if (_currentUserService.IsAuthenticated is false || _currentUserService.Current is null)
        {
            Context.Abort();
        }
        else
        {
            var result = await _chatsService.AddMessageAsync(chatId, _currentUserService.Current.Id, message,
                Context.ConnectionAborted);
            if (result.IsFailure) return;
            var createdMessage = result.Value.Messages.MaxBy(x => x.CreatedAt);
            if (createdMessage is null) return;
            await Clients
                .Group(chatId.ToString())
                .ReceiveMessageAsync(createdMessage.ToGet(), chatId, Context.ConnectionAborted);
        }
    }
}