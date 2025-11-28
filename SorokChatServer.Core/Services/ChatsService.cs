using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Repositories;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class ChatsService : IChatsService
{
    private const string UserNotInChat = "Користувач не в чаті";
    private const string MessageNotInChat = "Повідомлення не в чаті";

    private readonly IChatsRepository _chatsRepository;
    private readonly IUsersService _usersService;

    public ChatsService(IChatsRepository chatsRepository, IUsersService usersService)
    {
        _chatsRepository = chatsRepository;
        _usersService = usersService;
    }

    public async Task<Result<Chat>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _chatsRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<Chat>> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        return await _chatsRepository.GetByTitleAsync(title, cancellationToken);
    }

    public async Task<List<Chat>> GetByUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _chatsRepository.GetByUserAsync(userId, cancellationToken);
    }

    public async Task<Result<Chat>> CreateAsync(CreateChat createdChat, User author, User opponent,
        CancellationToken cancellationToken = default)
    {
        var chatResult = Chat.Create(createdChat.Title, createdChat.Description);
        if (chatResult.IsFailure) return chatResult;
        var chat = chatResult.Value;
        chat.AddMember(author);
        chat.AddMember(opponent);
        return await _chatsRepository.CreateAsync(chat, cancellationToken);
    }

    public async Task<Result<Chat>> AddUserAsync(long chatId, long userId,
        CancellationToken cancellationToken = default)
    {
        var userResult = await _usersService.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure) return Result.Failure<Chat>(userResult.Error);
        var chatResult = await _chatsRepository.GetByIdAsync(chatId, cancellationToken);
        if (chatResult.IsFailure) return chatResult;
        var chat = chatResult.Value;
        chat.AddMember(userResult.Value);
        return await _chatsRepository.UpdateAsync(chatId, chat, cancellationToken);
    }

    public async Task<Result<Chat>> RemoveUserAsync(long chatId, long userId,
        CancellationToken cancellationToken = default)
    {
        var userResult = await _usersService.GetByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure) return Result.Failure<Chat>(userResult.Error);
        var chatResult = await _chatsRepository.GetByIdAsync(chatId, cancellationToken);
        if (chatResult.IsFailure) return chatResult;
        var chat = chatResult.Value;
        chat.RemoveMember(userResult.Value);
        return await _chatsRepository.UpdateAsync(chatId, chat, cancellationToken);
    }

    public async Task<Result<Chat>> UpdateAsync(long id, UpdateChat updatedChat,
        CancellationToken cancellationToken = default)
    {
        var candidate = await GetByIdAsync(id, cancellationToken);
        if (candidate.IsFailure) return candidate;
        var chat = candidate.Value.ToEntity();
        var titleResult = Title.Create(updatedChat.Title);
        var descriptionResult = Description.Create(updatedChat.Description);
        if (titleResult.IsSuccess) chat.Title = titleResult.Value;
        if (descriptionResult.IsSuccess) chat.Description = descriptionResult.Value;
        return await _chatsRepository.UpdateAsync(id, Chat.FromEntity(chat), cancellationToken);
    }

    public async Task<Result<Chat>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _chatsRepository.DeleteAsync(id, cancellationToken);
    }
}