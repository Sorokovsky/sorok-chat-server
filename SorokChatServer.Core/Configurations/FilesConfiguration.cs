using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Options;

namespace SorokChatServer.Core.Configurations;

public class FilesConfiguration : StaticFileOptions
{
    public FilesConfiguration(IFilesService filesService, IOptionsMonitor<FilesOptions> options)
    {
        var info = new DirectoryInfo(filesService.StaticFolder);
        if (info.Exists is false) Directory.CreateDirectory(filesService.StaticFolder);
        RequestPath = options.CurrentValue.RequestPath;
        FileProvider = new PhysicalFileProvider(filesService.StaticFolder);
    }
}