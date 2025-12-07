using CSharpFunctionalExtensions;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public interface ITokenDeserializer
{
    Result<Token, Error> Deserialize(string token);
}