using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Contracts.User;
using SorokChatServer.Domain.Models;
using SorokChatServer.Domain.Repositories;

namespace SorokChatServer.Domain.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IUsersRepository _repository;

    public UsersService(IUsersRepository repository, IPasswordHasherService passwordHasherService)
    {
        _repository = repository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result<User, Error>> CreateAsync(NewUser newUser, CancellationToken cancellationToken = default)
    {
        var passwordResult = HashedPassword.Create(newUser.Password, _passwordHasherService);
        if (passwordResult.IsFailure) return passwordResult.Error;
        var userResult = User.Create(newUser.Email, passwordResult.Value, newUser.FirstName, newUser.LastName,
            newUser.MiddleName);
        if (userResult.IsFailure) return userResult.Error;
        return await _repository.CreateAsync(userResult.Value, cancellationToken);
    }

    public async Task<Result<User, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<User, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByEmailAsync(email, cancellationToken);
    }

    public async Task<Result<User, Error>> UpdateAsync(long id, UpdateUser updateUser,
        CancellationToken cancellationToken = default)
    {
        var candidateResult = await GetByIdAsync(id, cancellationToken);
        if (candidateResult.IsFailure) return candidateResult.Error;
        var candidate = candidateResult.Value;
        var passwordResult = HashedPassword.Create(updateUser.Password, _passwordHasherService);
        var password = passwordResult.IsSuccess ? passwordResult.Value : candidate.HashedPassword;
        var emailResult = Email.Create(updateUser.Email);
        var email = emailResult.IsSuccess ? emailResult.Value.Value : candidate.Email.Value;
        var fullNameResult = FullName.Create(updateUser.FirstName, updateUser.LastName, updateUser.MiddleName);
        var firstName = fullNameResult.IsSuccess ? fullNameResult.Value.FirstName : candidate.FullName.FirstName;
        var lastName = fullNameResult.IsSuccess ? fullNameResult.Value.LastName : candidate.FullName.LastName;
        var middleName = fullNameResult.IsSuccess ? fullNameResult.Value.MiddleName : candidate.FullName.MiddleName;
        var userResult = User.Create(email, password, firstName, lastName, middleName);
        if (userResult.IsFailure) return userResult.Error;
        var user = userResult.Value;
        user.SetId(candidate.Id);
        user.SetCreatedAt(candidate.CreatedAt);
        user.SetUpdatedAt(DateTime.UtcNow);
        return await _repository.UpdateAsync(id, user, cancellationToken);
    }

    public async Task<Result<User, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}