using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Repositories;

public interface IUsersRepository
{
    public Task<Result<User>> GetByIdAsync(long userId, CancellationToken cancellationToken = default);

    public Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    public Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default);

    public Task<Result<User>> UpdateAsync(long id, User user, CancellationToken cancellationToken = default);

    public Task<Result<User>> DeleteAsync(long userId, CancellationToken cancellationToken = default);
}