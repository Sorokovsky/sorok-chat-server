using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using SorokChatServer.Core.Entities;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task<Result<T, ApiError>> GetOneBy(Expression<Func<T, bool>> wherePredicate,
        CancellationToken cancellationToken);

    public Task<Result<IEnumerable<T>, ApiError>> GetManyBy(Expression<Func<T, bool>> wherePredicate,
        CancellationToken cancellationToken);

    public Task<Result<T, ApiError>> Create(T item, CancellationToken cancellationToken);

    public Task<Result<T, ApiError>> Update(Expression<Func<T, bool>> wherePredicate, T updatedItem,
        CancellationToken cancellationToken);

    public Task<Result<T, ApiError>> Delete(Expression<Func<T, bool>> wherePredicate,
        CancellationToken cancellationToken);
}