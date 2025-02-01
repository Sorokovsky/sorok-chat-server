using System.Linq.Expressions;
using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Core.Entities;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Models;
using SorokChatServer.Core.Utils;

namespace SorokChatServer.DataAccess.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly Database _database;
    private readonly DbSet<T> _items;

    public BaseRepository(Database database)
    {
        _database = database;
        _items = _database.Set<T>();
    }

    public async Task<Result<T, ApiError>> GetOneBy(Expression<Func<T, bool>> wherePredicate,
        CancellationToken cancellationToken)
    {
        var result = await _items.AsNoTracking().FirstOrDefaultAsync(wherePredicate, cancellationToken);
        if (result is null)
            return Result.Failure<T, ApiError>(new ApiError("Entity not found", HttpStatusCode.BadRequest));

        return Result.Success<T, ApiError>(result);
    }

    public async Task<Result<IEnumerable<T>, ApiError>> GetManyBy(Expression<Func<T, bool>> wherePredicate,
        CancellationToken cancellationToken)
    {
        var result = await _items.AsNoTracking().Where(wherePredicate).ToListAsync(cancellationToken);
        if (result.Count <= 0)
            return Result.Failure<IEnumerable<T>, ApiError>(new ApiError("List of entity is empty",
                HttpStatusCode.BadRequest));

        return Result.Success<IEnumerable<T>, ApiError>(result);
    }

    public async Task<Result<T, ApiError>> Create(T item, CancellationToken cancellationToken)
    {
        var result = (await _items.AddAsync(item, cancellationToken)).Entity;
        await _database.SaveChangesAsync(cancellationToken);
        return Result.Success<T, ApiError>(result);
    }

    public async Task<Result<T, ApiError>> Update(Expression<Func<T, bool>> wherePredicate, T updatedItem,
        CancellationToken cancellationToken)
    {
        var candidateResult = await GetOneBy(wherePredicate, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult;
        var newState = RepositoryUtils.MergeStates(candidateResult.Value, updatedItem)!;
        await using var transaction = await _database.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var local = _database.Set<T>().Local.FirstOrDefault(wherePredicate.Compile());
            if (local is not null) _database.Entry(local).State = EntityState.Detached;
            var result = _items.Update(newState).Entity;
            await _database.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success<T, ApiError>(result);
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<T, ApiError>(new ApiError(exception.Message, HttpStatusCode.InternalServerError));
        }
    }

    public async Task<Result<T, ApiError>> Delete(Expression<Func<T, bool>> wherePredicate,
        CancellationToken cancellationToken)
    {
        var result = await GetOneBy(wherePredicate, cancellationToken);
        if (result.IsFailure) return result;
        await _items.Where(wherePredicate).ExecuteDeleteAsync(cancellationToken);
        return result;
    }
}