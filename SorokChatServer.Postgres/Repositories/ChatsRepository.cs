using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Repositories;

namespace SorokChatServer.Postgres.Repositories;

public class ChatsRepository : IChatsRepository
{
    private const string NotFoundMessage = "Чат не знайдений";

    private readonly DatabaseContext _context;

    public ChatsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Chat>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Chats.AsNoTracking()
            .Include(chat => chat.Members)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (result is null) return Result.Failure<Chat>(NotFoundMessage);
        return Result.Success(Chat.FromEntity(result));
    }

    public async Task<Result<Chat>> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        var result = await _context.Chats.AsNoTracking()
            .Include(chat => chat.Members)
            .FirstOrDefaultAsync(x => x.Title.Value == title, cancellationToken);
        if (result is null) return Result.Failure<Chat>(NotFoundMessage);
        return Result.Success(Chat.FromEntity(result));
    }

    public async Task<List<Chat>> GetByUserAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Chats.AsNoTracking()
            .Include(chat => chat.Members)
            .Where(chat => chat.Members.Any(member => member.Id == userId))
            .Select(chat => Chat.FromEntity(chat))
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<Chat>> CreateAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        var entity = chat.ToEntity();
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            entity.Members.ForEach(member =>
            {
                if (_context.Entry(member).State != EntityState.Unchanged)
                    _context.Entry(member).State = EntityState.Unchanged;
            });
            var created = await _context.Chats.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success(Chat.FromEntity(created.Entity));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<Chat>(exception.Message);
        }
    }

    public async Task<Result<Chat>> UpdateAsync(long id, Chat chat, CancellationToken cancellationToken = default)
    {
        var candidate = await GetByIdAsync(id, cancellationToken);
        if (candidate.IsFailure) return candidate;
        var entity = chat.ToEntity();
        entity.Id = id;
        entity.UpdatedAt = DateTime.UtcNow;
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success(Chat.FromEntity(entity));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<Chat>(exception.Message);
        }
    }

    public async Task<Result<Chat>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var candidate = await GetByIdAsync(id, cancellationToken);
        if (candidate.IsFailure) return candidate;
        await _context.Chats.Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        return candidate;
    }

    public async Task<Result<Chat>> GetLastCreatedAsync(CancellationToken cancellationToken = default)
    {
        var candidate = await _context.Chats.AsNoTracking()
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (candidate is null) return Result.Failure<Chat>(NotFoundMessage);
        return Result.Success(Chat.FromEntity(candidate));
    }
}