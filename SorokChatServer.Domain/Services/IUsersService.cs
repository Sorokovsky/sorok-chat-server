using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Contracts.User;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public interface IUsersService
{
    Task<Result<User, Error>> CreateAsync(NewUser newUser, CancellationToken cancellationToken = default);
    Task<Result<User, Error>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Result<User, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<Result<User, Error>> UpdateAsync(long id, UpdateUser updateUser,
        CancellationToken cancellationToken = default);

    Task<Result<User, Error>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}