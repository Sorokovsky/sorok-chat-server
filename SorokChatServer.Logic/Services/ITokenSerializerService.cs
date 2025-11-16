using CSharpFunctionalExtensions;
using SorokChatServer.Logic.Contracts;

namespace SorokChatServer.Logic.Services;

public interface ITokenSerializerService
{
    Task<string> SerializeTokenAsync(Token token, CancellationToken cancellationToken = default);
    Task<Result<Token>> DeserializeTokenAsync(string token, CancellationToken cancellationToken = default);
}