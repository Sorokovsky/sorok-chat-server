using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Repositories;

public interface IChatsRepository
{
    public Task<Result<Chat>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<Result<Chat>> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
    public Task<List<Chat>> GetByUserAsync(long userId, CancellationToken cancellationToken = default);
    public Task<Result<Chat>> CreateAsync(Chat chat, CancellationToken cancellationToken = default);
    public Task<Result<Chat>> UpdateAsync(long id, Chat chat, CancellationToken cancellationToken = default);
    public Task<Result<Chat>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}