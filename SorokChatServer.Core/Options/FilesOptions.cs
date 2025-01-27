namespace SorokChatServer.Core.Options;

public class FilesOptions
{
    public static readonly string Files = nameof(Files);

    public string DirectoryPath { get; set; } = string.Empty;

    public string RequestPath { get; set; } = string.Empty;
}