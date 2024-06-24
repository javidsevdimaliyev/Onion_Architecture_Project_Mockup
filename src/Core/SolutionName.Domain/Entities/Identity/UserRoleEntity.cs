using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SolutionName.Domain.Entities.Common;

namespace SolutionName.Domain.Entities.Identity;

public class UserRoleEntity : IdentityUserRole<int>, IAuditableEntity<int>
{
    public int UserId { get; set; }   
    public int RoleId { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public long CreatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public long? UpdatedUserId { get; set; }
    public bool IsDeleted { get; set; }
    public int OrderIndex { get; set; }

    #region references

    public UserEntity User { get; set; }

    public RoleEntity Role { get; set; }

    #endregion
}