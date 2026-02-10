using System.ComponentModel.DataAnnotations;
namespace AuthService.Domain.Entities;
public class Role {
    [ket]
    [MaxLength(16)]
    public string Id {get; set; }
    [required]
    [MawLength(50)]
    public string Name {get; set; }
    [required]
    [MaxLength(255)]
    public string Description {get; set;}
    public ICollection<UserRole> UserRoles {get;set;}
}
