using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nourishify.API.Security.Domain.Models;

namespace Nourishify.API.Security.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Authorization process
        var info = (String)context.HttpContext.Items["Info"];
        if (info != "null")
        {
            context.Result = info == "Expired Token" ? 
                new JsonResult(new { message = "Expired Token" }) { StatusCode = StatusCodes.Status401Unauthorized } : 
                new JsonResult(new { message = "Invalid Token" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null || user.RoleId == 1) //If there is no user or the user has role of USER
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        
    }
}