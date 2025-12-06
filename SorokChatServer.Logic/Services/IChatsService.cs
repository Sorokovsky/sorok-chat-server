using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Services;

public interface IChatsService
{
    public Task<Result<Chat>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<Result<Chat>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    public Task<List<Chat>> GetByUserAsync(long userId, CancellationToken cancellationToken = default);

    public Task<Result<Chat>> CreateAsync(CreateChat createdChat, User author, User opponent,
        CancellationToken cancellationToken = default);

    public Task<Result<Chat>> AddUserAsync(long chatId, long userId, CancellationToken cancellationToken = default);
    public Task<Result<Chat>> RemoveUserAsync(long chatId, long userId, CancellationToken cancellationToken = default);

    public Task<Result<Chat>> UpdateAsync(long id, UpdateChat updatedChat,
        CancellationToken cancellationToken = default);

    public Task<Result<Chat>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}