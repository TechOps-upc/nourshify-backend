using Nourishify.API.Shared.Domain.Repositories;
using Nourishify.API.Shared.Persistence.Context;

namespace Nourishify.API.Shared.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}