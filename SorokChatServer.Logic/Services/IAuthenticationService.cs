using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Services;

public interface IAuthenticationService
{
    public Task<Result<User>> RegisterAsync(CreateUser createdUser, CancellationToken cancellationToken = default);

    public Task<Result<User>> LoginAsync(LoginUser loginUser, CancellationToken cancellationToken = default);

    public Task LogoutAsync(CancellationToken cancellationToken = default);
}