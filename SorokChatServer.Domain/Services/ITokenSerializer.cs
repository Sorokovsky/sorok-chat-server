using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public interface ITokenSerializer
{
    public Result<string, Error> Serialize(Token token);
}