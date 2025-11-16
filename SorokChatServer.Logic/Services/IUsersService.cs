using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Services;

public interface IUsersService
{
    public Task<Result<User>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    public Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    public Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default);

    public Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken = default);

    public Task<Result<User>> DeleteAsync(long userId, CancellationToken cancellationToken = default);
}