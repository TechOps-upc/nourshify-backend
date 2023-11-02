namespace Nourishify.API.Security.Domain.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    //Relationship
    public IList<User> Users { get; set; } = new List<User>();
}