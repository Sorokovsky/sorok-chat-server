using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using SorokChatServer.Core.Entities;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Interfaces;

public interface IUsersRepository
{
    public Task<Result<User, ApiError>> Create(User newUser, CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> GetBy(Expression<Func<UserEntity, bool>> filter,
        CancellationToken cancellationToken);

    public Task<Result<List<User>, ApiError>> GetMany(Expression<Func<UserEntity, bool>> filter,
        CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> Delete(long id, CancellationToken cancellationToken);

    public Task<Result<User, ApiError>> Update(long id, User updatedUser, CancellationToken cancellationToken);
}