using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Repositories;

public interface IBaseRepository<T> where T : Base
{
    Task<Result<T, Error>> CreateAsync(T model, CancellationToken cancellationToken = default);
    Task<Result<T, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<T>> GetManyAsync(int limit, int offset, CancellationToken cancellationToken = default);
    Task<Result<T, Error>> UpdateAsync(long id, T model, CancellationToken cancellationToken = default);
    Task<Result<T, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}