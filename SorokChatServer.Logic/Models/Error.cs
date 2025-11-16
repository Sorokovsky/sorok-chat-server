using System.Net;

namespace SorokChatServer.Logic.Models;

public class Error
{
    public HttpStatusCode StatusCode { get; }
    public string Message { get; }

    public Error(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}