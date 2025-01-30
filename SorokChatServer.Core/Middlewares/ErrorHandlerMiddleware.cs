using System.Net;
using Microsoft.AspNetCore.Http;
using SorokChatServer.Core.Models;

namespace SorokChatServer.Core.Middlewares;

public class ErrorHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var error = new ApiError(e.Message, HttpStatusCode.InternalServerError);
            await SendAsync(context, error);
        }
    }

    private static async Task SendAsync(HttpContext context, ApiError error)
    {
        var response = context.Response;
        response.StatusCode = (int)error.StatusCode;
        await response.WriteAsJsonAsync(error);
    }
}