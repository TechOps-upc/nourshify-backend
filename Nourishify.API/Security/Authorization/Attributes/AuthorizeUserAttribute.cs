using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nourishify.API.Security.Domain.Models;

namespace Nourishify.API.Security.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        /*// If action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        // Then skip authorization process
        if (allowAnonymous)
            return;*/

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
            if (user == null ) 
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            else if(user.RoleId is 1 or 2) { var dummy = 2; }
        }
        
    }
}