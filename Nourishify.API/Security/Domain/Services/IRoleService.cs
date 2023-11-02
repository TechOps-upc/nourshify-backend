using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Services.Communication;

namespace Nourishify.API.Security.Domain.Services;

public interface IRoleService
{
    Task<IEnumerable<Role>> ListAsync();
    Task<RoleResponse> SaveAsync(Role role);
}