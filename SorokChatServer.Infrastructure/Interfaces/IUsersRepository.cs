using CSharpFunctionalExtensions;

namespace SorokChatServer.Infrastructure.Interfaces;

public interface IUsersRepository
{
    public Task<Result<>> GetAll
}