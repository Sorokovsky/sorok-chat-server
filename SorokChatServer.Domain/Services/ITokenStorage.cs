using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public interface ITokenStorage
{
    Result<Token, Error> GetToken(HttpRequest request);
    void SetToken(Token token, HttpResponse response);
    void ClearToken(HttpResponse response);
}