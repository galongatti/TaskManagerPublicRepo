using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using src.TaskManagerBackEnd;
using TaskManagerBackEnd.Service;

namespace TaskManagerBackEnd.Authorize;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        object? userContextItem = context.HttpContext.Items["User"];

        if (userContextItem != null)
        {
            string email = (string)userContextItem;
            CheckPermission(email, context);
        }
        else
        {
            SetResultUnauthorized(context);
        }
    }

    private void CheckPermission(string email, AuthorizationFilterContext context)
    {
        IUserService? userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

        User? user = userService.GetUserByEmail(email);

        if (user == null)
        {
            SetResultUnauthorized(context);
        }
    }
    private void SetResultUnauthorized(AuthorizationFilterContext context)
    {
        context.Result = new JsonResult(new { message = "Unauthorized" })
            { StatusCode = StatusCodes.Status401Unauthorized };
    }
}