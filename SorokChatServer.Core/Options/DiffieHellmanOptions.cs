using System.Numerics;

namespace SorokChatServer.Core.Options;

public class DiffieHellmanOptions
{
    public BigInteger G { get; set; }

    public BigInteger P { get; set; }
}