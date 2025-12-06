using System.Net;

namespace SorokChatServer.Domain.Models;

public record Error(string Message, HttpStatusCode StatusCode);