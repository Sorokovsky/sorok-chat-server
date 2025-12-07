using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Domain.Models;

namespace SorokChatServer.Domain.Services;

public class BearerTokenStorage : IAccessTokenStorage
{
    private const string Authorization = nameof(Authorization);
    private const string EmptyError = "Токен доступу не може бути відсутнім.";
        
    private readonly ITokenSerializer _tokenSerializer;
    private readonly ITokenDeserializer _tokenDeserializer;

    public BearerTokenStorage(ITokenSerializer tokenSerializer, ITokenDeserializer tokenDeserializer)
    {
        _tokenSerializer = tokenSerializer;
        _tokenDeserializer = tokenDeserializer;
    }

    public Result<Token, Error> GetToken(HttpRequest request)
    {
        var authorizationHeader = request.Headers[Authorization].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return new Error(EmptyError, HttpStatusCode.Unauthorized);
        }
        
        var (_, isFailure, token, error) = _tokenDeserializer.Deserialize(authorizationHeader);
        if (isFailure) return error;
        return token;
    }

    public void SetToken(Token token, HttpResponse response)
    {
        var tokenString = _tokenSerializer.Serialize(token);
        if (tokenString.IsSuccess)
        {
            response.Headers[Authorization] = tokenString.Value;
        }
    }

    public void ClearToken(HttpResponse response)
    {
        response.Headers.Remove(Authorization);
    }
}