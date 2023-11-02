using Microsoft.EntityFrameworkCore;
using Nourishify.API.Security.Domain.Models;

namespace Nourishify.API.Shared.Persistence.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}