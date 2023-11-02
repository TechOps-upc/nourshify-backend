using Nourishify.API.Security.Domain.Models;

namespace Nourishify.API.Security.Authorization.Handlers.Interfaces;

public interface IJwtHandler
{
    public string GenerateToken(User user);
    public int? ValidateToken(string token);
}