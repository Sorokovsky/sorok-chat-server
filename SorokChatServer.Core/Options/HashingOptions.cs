namespace SorokChatServer.Core.Options;

public class HashingOptions
{
    public static readonly string Hashing = nameof(Hashing);

    public int Rounds { get; set; }
}