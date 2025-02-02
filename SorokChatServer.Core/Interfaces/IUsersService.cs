using CSharpFunctionalExtensions;
using SorokChatServer.Core.Contracts;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Interfaces;

public interface IUsersService
{
    public Task<Result<User, ApiError>> Create(CreateUserRequest newUser, CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> GetById(long id, CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> Update(long id, UpdateUserRequest user, CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> Delete(long id, CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> DeleteAvatar(long id, CancellationToken cancellationToken);
}