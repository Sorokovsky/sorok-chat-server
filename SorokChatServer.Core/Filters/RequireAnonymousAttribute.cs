using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SorokChatServer.Core.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireAnonymousAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated == true)
            context.Result = new ObjectResult(new
            {
                error = "Ви вже авторизовані."
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
    }
}