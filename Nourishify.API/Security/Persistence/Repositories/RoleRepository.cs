using Microsoft.EntityFrameworkCore;
using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Repositories;
using Nourishify.API.Shared.Persistence.Context;
using Nourishify.API.Shared.Persistence.Repositories;

namespace Nourishify.API.Security.Persistence.Repositories;

public class RoleRepository : BaseRepository, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Role>> ListAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Role> FindByIdAsync(int id)
    {
        return await _context.Roles.FindAsync(id);
    }

    public async Task AddAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
    }
}