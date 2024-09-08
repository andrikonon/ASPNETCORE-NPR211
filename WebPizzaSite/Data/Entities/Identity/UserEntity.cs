using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebPizzaSite.Data.Entities.Identity;


[Table("tblUsers")]
public class UserEntity : IdentityUser<int>
{
    [StringLength(50)]
    public string? LastName { get; set; }
    [StringLength(50)]
    public string? FirstName { get; set; }
    [StringLength(100)]
    public string? Picture { get; set; }
    public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
}