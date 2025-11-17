using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Repositories;

public interface IMessagesRepository
{
    public Task<Result<Message>> CreateAsync(Message message, CancellationToken cancellationToken = default);
    public Task<Result<Message>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    public Task<Result<Message>> UpdateAsync(long id, Message message, CancellationToken cancellationToken = default);
    public Task<Result<Message>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}