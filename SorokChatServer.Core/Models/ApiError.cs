using System.Net;
using System.Text.Json.Serialization;

namespace SorokChatServer.Core.Models;

public class ApiError
{
    public ApiError(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    [JsonPropertyName("message")] public string Message { get; }

    [JsonPropertyName("statusCode")] public HttpStatusCode StatusCode { get; }
}