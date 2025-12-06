using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using SorokChatServer.Domain.Models;
using SorokChatServer.Domain.Repositories;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres.Repositories;

public class BaseRepository<TModel, TEntity> : IBaseRepository<TModel> where TModel : Base where TEntity : BaseEntity
{

    private const string UnknownError = "Невідома помилка";
    
    private readonly PostgresContext _context;
    private readonly ILogger<BaseRepository<TModel, TEntity>> _logger;

    public BaseRepository(PostgresContext context, ILogger<BaseRepository<TModel, TEntity>> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<TModel, Error>> CreateAsync(
        TModel model, 
        CancellationToken cancellationToken = default
        )
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<TModel, Error>(new Error(UnknownError, HttpStatusCode.InternalServerError));
        }
    }

    public Task<Result<TModel, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<TModel>> GetManyAsync(long limit, long offset, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TModel, Error>> UpdateAsync(long id, TModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TModel, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}