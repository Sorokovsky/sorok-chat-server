using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Repositories;

public interface IUsersRepository : IBaseRepository<User>
{
    Task<Result<User, Error>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}