using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Services;

public interface IUsersService
{
    public Task<Result<User>> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    public Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    public Task<Result<User>> CreateAsync(CreateUser createdUser, CancellationToken cancellationToken = default);

    public Task<Result<User>> UpdateAsync(long id, UpdateUser updatedUser,
        CancellationToken cancellationToken = default);

    public Task<Result<User>> SetPublicRsaKey(long id, string publicRsaKey,
        CancellationToken cancellationToken = default);

    public Task<Result<User>> DeleteAsync(long userId, CancellationToken cancellationToken = default);
}