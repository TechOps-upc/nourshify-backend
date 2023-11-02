using System.ComponentModel.DataAnnotations;

namespace Nourishify.API.Security.Domain.Services.Communication;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Address { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Plan { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Photo { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public int RoleId { get; set; }
}