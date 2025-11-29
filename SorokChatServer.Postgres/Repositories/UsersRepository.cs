using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Repositories;

namespace SorokChatServer.Postgres.Repositories;

public class UsersRepository : IUsersRepository
{
    private const string NotFoundError = "Користувача не знайден";
    private const string AlreadyExists = "Користувач вже існує.";

    private readonly DatabaseContext _context;

    public UsersRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var found = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (found is null) return Result.Failure<User>(NotFoundError);
        return Result.Success(User.FromEntity(found));
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var found = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Value == email, cancellationToken);
        if (found is null) return Result.Failure<User>(NotFoundError);
        return Result.Success(User.FromEntity(found));
    }

    public async Task<Result<User>> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var candidate = await GetByEmailAsync(user.Email.Value, cancellationToken);
        if (candidate.IsSuccess) return Result.Failure<User>(AlreadyExists);
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var saved = await _context.Users.AddAsync(user.ToEntity(), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success(User.FromEntity(saved.Entity));
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<User>(exception.Message);
        }
    }

    public async Task<Result<User>> UpdateAsync(long id, User user, CancellationToken cancellationToken = default)
    {
        var candidate = await GetByIdAsync(id, cancellationToken);
        if (candidate.IsFailure) return Result.Failure<User>(NotFoundError);
        var entity = user.ToEntity();
        entity.Id = id;
        entity.UpdatedAt = DateTime.UtcNow;
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return Result.Success(User.FromEntity(entity));
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            return Result.Failure<User>(exception.Message);
        }
    }

    public async Task<Result<User>> DeleteAsync(long userId, CancellationToken cancellationToken = default)
    {
        var found = await GetByIdAsync(userId, cancellationToken);
        if (found.IsFailure) return Result.Failure<User>(NotFoundError);
        await _context.Users.Where(user => user.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(found.Value);
    }
}