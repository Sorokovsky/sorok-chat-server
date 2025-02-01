using System.Linq.Expressions;
using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Core.Entities;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Models;
using SorokChatServer.Core.Utils;

namespace SorokChatServer.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly Database _database;

    public UsersRepository(Database database)
    {
        _database = database;
    }

    public async Task<Result<User, ApiError>> Create(User newUser, CancellationToken cancellationToken)
    {
        var candidate = await GetBy(user => user.Email == newUser.Email, cancellationToken);
        if (candidate.IsSuccess)
        {
            var error = new ApiError("User by this email already exists.", HttpStatusCode.BadRequest);
            return Result.Failure<User, ApiError>(error);
        }

        try
        {
            var entity = newUser.ToEntity();
            var createdUser = (await _database.Users.AddAsync(entity, cancellationToken)).Entity;
            await _database.SaveChangesAsync(cancellationToken);
            return Result.Success<User, ApiError>(User.FromEntity(createdUser));
        }
        catch (Exception e)
        {
            return Result.Failure<User, ApiError>(new ApiError(e.Message, HttpStatusCode.InternalServerError));
        }
    }

    public async Task<Result<User, ApiError>> GetBy(Expression<Func<UserEntity, bool>> filter,
        CancellationToken cancellationToken)
    {
        var user = await _database.Users.AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
        if (user is null)
        {
            var error = new ApiError("User not found.", HttpStatusCode.BadRequest);
            return Result.Failure<User, ApiError>(error);
        }

        return Result.Success<User, ApiError>(User.FromEntity(user));
    }

    public async Task<Result<List<User>, ApiError>> GetMany(Expression<Func<UserEntity, bool>> filter,
        CancellationToken cancellationToken)
    {
        var users = await _database.Users
            .AsNoTracking()
            .Where(filter)
            .Select(x => User.FromEntity(x))
            .ToListAsync(cancellationToken);
        if (users.Count <= 0)
            return Result.Failure<List<User>, ApiError>(new ApiError("Users count is 0", HttpStatusCode.BadRequest));

        return Result.Success<List<User>, ApiError>(users);
    }

    public async Task<Result<User, ApiError>> Delete(long id, CancellationToken cancellationToken)
    {
        var candidateResult = await GetBy(x => x.Id == id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        await _database.Users
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        return Result.Success<User, ApiError>(candidateResult.Value);
    }

    public async Task<Result<User, ApiError>> Update(long id, User updatedUser,
        CancellationToken cancellationToken)
    {
        var candidateResult = await GetBy(x => x.Id == id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        var updatedState = RepositoryUtils.MergeStates(candidateResult.Value.ToEntity(), updatedUser);
        updatedState.UpdatedAt = DateTime.UtcNow;
        var local = _database.Set<UserEntity>()
            .Local.FirstOrDefault(x => x.Id == id);
        if (local is not null) _database.Entry(local).State = EntityState.Detached;
        _database.Users.Attach(updatedState);
        await _database.SaveChangesAsync(cancellationToken);
        return Result.Success<User, ApiError>(User.FromEntity(updatedState));
    }
}