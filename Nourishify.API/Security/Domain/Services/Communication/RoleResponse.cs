using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Shared.Domain.Services.Communication;

namespace Nourishify.API.Security.Domain.Services.Communication;

public class RoleResponse : BaseResponse<Role>
{
    public RoleResponse(string message) : base(message)
    {
    }

    public RoleResponse(Role model) : base(model)
    {
    }
}