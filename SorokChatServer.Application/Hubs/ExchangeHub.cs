using System.Numerics;
using Microsoft.AspNetCore.SignalR;
using SorokChatServer.Logic.Hubs;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Application.Hubs;

public class ExchangeHub : Hub<IExchangeHub>
{
    private readonly IDiffieHellmanService _diffieHellmanService;
    private readonly ITripleDiffieHellmanService _tripleDiffieHellmanService;

    public ExchangeHub(
        IDiffieHellmanService diffieHellmanService,
        ITripleDiffieHellmanService tripleDiffieHellmanService
    )
    {
        _diffieHellmanService = diffieHellmanService;
        _tripleDiffieHellmanService = tripleDiffieHellmanService;
    }

    public async Task Exchange(BigInteger otherStaticPublicKey, BigInteger otherEphemeralPublicKey)
    {
        var myStaticKeys = _diffieHellmanService.GenerateKeysPair();
        var myEphemeralKeys = _diffieHellmanService.GenerateKeysPair();
        var staticKeys = myStaticKeys with { PublicKey = otherStaticPublicKey };
        var ephemeralKeys = myEphemeralKeys with { PublicKey = otherEphemeralPublicKey };
        var sharedKey = _tripleDiffieHellmanService.GenerateSharedKey(staticKeys, ephemeralKeys);
        await Clients.Caller.ReceiveExchange(myStaticKeys.PublicKey, myEphemeralKeys.PublicKey);
    }
}