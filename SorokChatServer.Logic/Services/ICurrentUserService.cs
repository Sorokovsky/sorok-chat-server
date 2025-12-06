using SorokChatServer.Logic.Models;

namespace SorokChatServer.Logic.Services;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    User? Current { get; }
}