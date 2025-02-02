using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SorokChatServer.Core.Interfaces;
using SorokChatServer.Core.Models;
using SorokChatServer.Core.Options;

namespace SorokChatServer.Core.Services;

public class FilesService : IFilesService
{
    private readonly IOptionsMonitor<FilesOptions> _options;
    private readonly IWebHostEnvironment _environment;

    public FilesService(IOptionsMonitor<FilesOptions> options, IWebHostEnvironment environment)
    {
        _options = options;
        _environment = environment;
    }

    public string StaticFolder => Path.Combine(_environment.ContentRootPath, _options.CurrentValue.DirectoryPath);

    public async Task<Result<string, ApiError>> Upload(
        IFormFile file,
        string folder,
        string name,
        bool rewrite,
        CancellationToken cancellationToken
    )
    {
        var serverFolder = Path.Combine(StaticFolder, folder);
        var fileName = string.Concat(name, Path.GetExtension(file.FileName));
        var resultPath = Path.Combine(folder, fileName);
        var serverPath = Path.Combine(serverFolder, fileName);
        if (Directory.Exists(serverFolder) is false)
            try
            {
                Directory.CreateDirectory(serverFolder);
            }
            catch (Exception e)
            {
                var error = new ApiError(e.Message, HttpStatusCode.InternalServerError);
                return await Task.FromResult(Result.Failure<string, ApiError>(error));
            }

        if (File.Exists(serverPath))
        {
            var error = new ApiError("File is already exists.", HttpStatusCode.BadRequest);
            if (rewrite is false) return await Task.FromResult(Result.Failure<string, ApiError>(error));
            File.Delete(serverPath);
        }

        try
        {
            await using var stream = File.Open(serverPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
        }
        catch (Exception e)
        {
            return await Task.FromResult(
                Result.Failure<string, ApiError>(new ApiError(e.Message, HttpStatusCode.BadRequest)));
        }

        return await Task.FromResult(Result.Success<string, ApiError>(resultPath));
    }

    public async Task<Result<bool, ApiError>> Delete(string path, CancellationToken cancellationToken)
    {
        try
        {
            var serverPath = Path.Combine(StaticFolder, path);
            var attributes = File.GetAttributes(serverPath);
            var isExists = Directory.Exists(serverPath) || File.Exists(serverPath);
            if (isExists is false)
            {
                var error = new ApiError("File of directory do not exists.", HttpStatusCode.BadRequest);
                return await Task.FromResult(Result.Failure<bool, ApiError>(error));
            }

            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                Directory.Delete(serverPath);
            else
                File.Delete(serverPath);

            return await Task.FromResult(Result.Success<bool, ApiError>(true));
        }
        catch (Exception e)
        {
            var error = new ApiError(e.Message, HttpStatusCode.InternalServerError);
            return await Task.FromResult(Result.Failure<bool, ApiError>(error));
        }
    }
}