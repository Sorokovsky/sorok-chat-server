using System.Numerics;
using Microsoft.AspNetCore.SignalR;
using SorokChatServer.Logic.Hubs;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Hubs;

public class ExchangeHub : Hub<IExchangeHub>
{
    private readonly IDiffieHellmanService _diffieHellmanService;

    public ExchangeHub(IDiffieHellmanService diffieHellmanService, IChatsService chatsService)
    {
        _diffieHellmanService = diffieHellmanService;
    }

    public async Task Exchange(BigInteger staticPublicKey, BigInteger ephemeralPublicKey)
    {
        var staticKeys = _diffieHellmanService.GenerateKeysPair();
        var ephemeralKeys = _diffieHellmanService.GenerateKeysPair();
        await Clients.Caller.ReceiveExchange(staticKeys.PublicKey, ephemeralKeys.PublicKey);
    }
}