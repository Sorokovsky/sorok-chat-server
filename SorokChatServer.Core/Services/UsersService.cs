using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;
using SorokChatServer.Logic.Repositories;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _repository;

    public UsersService(IUsersRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<User>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByEmailAsync(email, cancellationToken);
    }

    public async Task<Result<User>> CreateAsync(CreateUser createdUser, CancellationToken cancellationToken = default)
    {
        var passwordResult = await HashedPassword.Create(createdUser.Password, _passwordHasher);
        if (passwordResult.IsFailure) return Result.Failure<User>(passwordResult.Error);

        var created = User.Create(
            createdUser.FirstName,
            createdUser.LastName,
            createdUser.MiddleName,
            createdUser.Email,
            passwordResult.Value,
            createdUser.PublicRsaKey
        );
        if (created.IsFailure) return Result.Failure<User>(created.Error);
        return await _repository.CreateAsync(created.Value, cancellationToken);
    }

    public async Task<Result<User>> UpdateAsync(long id, UpdateUser updatedUser,
        CancellationToken cancellationToken = default)
    {
        var candidate = await _repository.GetByIdAsync(id, cancellationToken);
        if (candidate.IsFailure) return Result.Failure<User>(candidate.Error);
        var entity = candidate.Value.ToEntity();
        var nameResult = Name.Create(updatedUser.FirstName, updatedUser.LastName, updatedUser.MiddleName);
        var emailResult = Email.Create(updatedUser.Email);
        var passwordResult = await HashedPassword.Create(updatedUser.Password, _passwordHasher);
        if (nameResult.IsSuccess) entity.Name = nameResult.Value;
        if (emailResult.IsSuccess) entity.Email = emailResult.Value;
        if (passwordResult.IsSuccess) entity.Password = passwordResult.Value;
        return await _repository.UpdateAsync(id, User.FromEntity(entity), cancellationToken);
    }

    public async Task<Result<User>> SetPublicRsaKey(long id, string publicRsaKey,
        CancellationToken cancellationToken = default)
    {
        var userResult = await _repository.GetByIdAsync(id, cancellationToken);
        if (userResult.IsFailure) return Result.Failure<User>(userResult.Error);
        var user = userResult.Value.ToEntity();
        user.PublicRsaKey = publicRsaKey;
        var saved = await _repository.UpdateAsync(id, User.FromEntity(user), cancellationToken);
        if (saved.IsFailure) return Result.Failure<User>(saved.Error);
        return Result.Success(saved.Value);
    }

    public async Task<Result<User>> DeleteAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(userId, cancellationToken);
    }
}