using Nourishify.API.Security.Domain.Models;

namespace Nourishify.API.Security.Domain.Repositories;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> ListAsync();
    Task<Role> FindByIdAsync(int id);
    Task AddAsync(Role role);
}