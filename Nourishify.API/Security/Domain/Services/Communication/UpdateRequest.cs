namespace Nourishify.API.Security.Domain.Services.Communication;

public class UpdateRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Plan { get; set; }
    public string Photo { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}