using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Repositories;

namespace SorokChatServer.Postgres.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private const string NotFoundMessage = "Повідомлення не знайдено";

    private readonly DatabaseContext _context;

    public MessagesRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<Message>> CreateAsync(Message message, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var createdMessage = await _context.Messages
                .AddAsync(message.ToEntity(), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success(Message.FromEntity(createdMessage.Entity));
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<Message>(exception.Message);
        }
    }

    public async Task<Result<Message>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var message = await _context.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (message is null) return Result.Failure<Message>(NotFoundMessage);
        return Result.Success(Message.FromEntity(message));
    }

    public async Task<Result<Message>> UpdateAsync(long id, Message message,
        CancellationToken cancellationToken = default)
    {
        var candidate = await GetByIdAsync(id, cancellationToken);
        if (candidate.IsFailure) return candidate;
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var entity = message.ToEntity();
            entity.Id = id;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Messages.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success(Message.FromEntity(entity));
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<Message>(exception.Message);
        }
    }

    public async Task<Result<Message>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var messageResult = await GetByIdAsync(id, cancellationToken);
        if (messageResult.IsFailure) return messageResult;
        await _context.Messages.Where(m => m.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        return messageResult;
    }
}