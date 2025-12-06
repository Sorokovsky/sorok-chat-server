using System.Net;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public BaseRepository(
        PostgresContext context, 
        ILogger<BaseRepository<TModel, TEntity>> logger,
        IMapper mapper
        )
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Result<TModel, Error>> CreateAsync(
        TModel model, 
        CancellationToken cancellationToken = default
        )
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var entity = _mapper.Map<TEntity>(model);
            var createdEntity = await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TModel>(createdEntity.Entity);
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