using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Interfaces;

public interface IFilesService
{
    public Task<Result<string, ApiError>> Upload(
        IFormFile file,
        string folder,
        string name,
        bool rewrite,
        CancellationToken cancellationToken
    );

    public Task<Result<bool, ApiError>> Delete(string path, CancellationToken cancellationToken);

    public string StaticFolder { get; }
}