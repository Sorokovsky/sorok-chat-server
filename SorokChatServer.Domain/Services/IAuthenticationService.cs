using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Contracts;
using SorokChatServer.Domain.Contracts.User;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public interface IAuthenticationService
{
     Task<Result<User, Error>> RegisterAsync(NewUser newUser, CancellationToken cancellationToken = default);
     Task<Result<User, Error>> LoginAsync(LoginUser loginUser, CancellationToken cancellationToken = default);
     Task LogoutAsync(CancellationToken cancellationToken = default);
}