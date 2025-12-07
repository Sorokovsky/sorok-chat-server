using System.Net;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Domain.Models;
using SorokChatServer.Domain.Repositories;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres.Repositories;

public abstract class BaseRepository<TModel, TEntity> : IBaseRepository<TModel> where TModel : Base where TEntity : BaseEntity
{
    private const string NotFoundError = "Не знайдено.";
    
    private readonly PostgresContext _context;
    private readonly DbSet<TEntity> _items;
    private readonly IMapper _mapper;

    protected BaseRepository(
        PostgresContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
        _items = _context.Set<TEntity>();
    }

    public async Task<Result<TModel, Error>> CreateAsync(
        TModel model,
        CancellationToken cancellationToken = default
    )
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var entity = _mapper.Map<TEntity>(model);
        var createdEntity = await _items.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TModel>(createdEntity.Entity);
    }

    public async Task<Result<TModel, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var candidate = await _items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (candidate is null) return new Error(NotFoundError, HttpStatusCode.BadRequest);
        return _mapper.Map<TModel>(candidate);
    }

    public async Task<List<TModel>> GetManyAsync(int limit, int offset, CancellationToken cancellationToken = default)
    {
        return await _items
            .AsNoTracking()
            .Take(limit)
            .Skip(offset)
            .Select(x => _mapper.Map<TModel>(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<TModel, Error>> UpdateAsync(long id, TModel model,
        CancellationToken cancellationToken = default)
    {
        var candidateResult = await GetByIdAsync(id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        var mergedState = Merge(candidateResult.Value, model);
        var entity = _mapper.Map<TEntity>(mergedState);
        var updated = _items.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TModel>(updated.Entity);
    }

    public async Task<Result<TModel, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var candidateResult = await GetByIdAsync(id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        await _items.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<TModel>(candidateResult.Value);
    }

    protected abstract TModel Merge(TModel old, TModel current);
}