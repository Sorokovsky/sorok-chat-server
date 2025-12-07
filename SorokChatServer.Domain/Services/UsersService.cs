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

    public Task<Result<User, Error>> CreateAsync(NewUser newUser, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<User, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Result<User, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByEmailAsync(email, cancellationToken);
    }

    public Task<Result<User, Error>> UpdateAsync(long id, UpdateUser updateUser,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<User, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _repository.DeleteAsync(id, cancellationToken);
    }
}