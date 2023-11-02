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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // SECURITY
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<Role>().HasKey(r => r.Id);
        builder.Entity<Role>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Role>().Property(r => r.Name).IsRequired().HasMaxLength(30);
        
        // Role - User relationship
        builder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);
        
        // Constraints
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().HasKey(p => p.Id);
        builder.Entity<User>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(p => p.Username).IsRequired().HasMaxLength(30);
        builder.Entity<User>().Property(p => p.FirstName).IsRequired();
        builder.Entity<User>().Property(p => p.LastName).IsRequired();
        
    }
}