using System.Net;
using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Domain.Models;
using SorokChatServer.Domain.Repositories;
using SorokChatServer.Persistence.Postgres.Entities;

namespace SorokChatServer.Persistence.Postgres.Repositories;

public abstract class BaseRepository<TModel, TEntity> : IBaseRepository<TModel>
    where TModel : Base where TEntity : BaseEntity
{
    protected const string NotFoundError = "Не знайдено.";

    private readonly PostgresContext _context;
    protected readonly DbSet<TEntity> Items;
    protected readonly IMapper Mapper;

    protected BaseRepository(
        PostgresContext context,
        IMapper mapper
    )
    {
        _context = context;
        Mapper = mapper;
        Items = _context.Set<TEntity>();
    }

    public async Task<Result<TModel, Error>> CreateAsync(
        TModel model,
        CancellationToken cancellationToken = default
    )
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        var entity = Mapper.Map<TEntity>(model);
        var createdEntity = await Items.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Mapper.Map<TModel>(createdEntity.Entity);
    }

    public async Task<Result<TModel, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var candidate = await Items
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (candidate is null) return new Error(NotFoundError, HttpStatusCode.BadRequest);
        return Mapper.Map<TModel>(candidate);
    }

    public async Task<List<TModel>> GetManyAsync(int limit, int offset, CancellationToken cancellationToken = default)
    {
        return await Items
            .AsNoTracking()
            .Take(limit)
            .Skip(offset)
            .Select(x => Mapper.Map<TModel>(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<TModel, Error>> UpdateAsync(long id, TModel model,
        CancellationToken cancellationToken = default)
    {
        var candidateResult = await GetByIdAsync(id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        var mergedState = Merge(candidateResult.Value, model);
        var entity = Mapper.Map<TEntity>(mergedState);
        var updated = Items.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Mapper.Map<TModel>(updated.Entity);
    }

    public async Task<Result<TModel, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var candidateResult = await GetByIdAsync(id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        await Items.Where(x => x.Id == id).ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Mapper.Map<TModel>(candidateResult.Value);
    }

    protected abstract TModel Merge(TModel old, TModel current);
}