using System.ComponentModel.DataAnnotations;
 
namespace AuthService.Domain.Entities;
 
public class User
{
    [Key]
    [MaxLength(16)]
    public string Id {get; set; }
 
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(25)]
    public string Name {get; set; } = string.Empty;
 
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [MaxLength(25)]
    public string Surname {get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Username {get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email {get; set; } = string.Empty;
 
    [Required]
    [MaxLength(255)]
    public string Password {get; set; } = string.Empty;
 
    public bool Status {get; set; } = true;

    [Required]
    public DateTime CreatedAt {get;set;}

    [Required]
    public DateTime UpdatedAt {get;set;}

    public UserProfile UserProfile {get;set;} = null!;
    public ICollection<UserRole> UserRoles {get; set; } = [];
    public UserEmail UserEmail {get;set;} = null!;
    public UserPasswordReset PasswordReset {get;set;} = null!;
}