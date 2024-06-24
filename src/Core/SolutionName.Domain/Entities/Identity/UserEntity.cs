using Microsoft.AspNetCore.Identity;
using SolutionName.Domain.Entities.Common;
using SolutionName.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Domain.Entities.Identity;

public class UserEntity : IdentityUser<int>, IAuditableEntity<int>
{
  
    [NotMapped] public string FullName => $"{Surname} {Name}";

    [MaxLength(50)] 
    public string? Name { get; set; }

    [MaxLength(50)] 
    public string? Surname { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }

    public AuthenticationTypeEnum? AuthenticationType { get; set; }

    [Timestamp] 
    public byte[] TimeStamp { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int CreatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int? UpdatedUserId { get; set; }
    public bool IsDeleted { get; set; }
    public int OrderIndex { get; set; } = 0;
    //Refresh Token Columns
    public string RefreshToken { get; set; }
    public DateTime TokenCreatedDate { get; set; }
    public DateTime TokenExpireDate { get; set; }


    #region references
    public ICollection<UserRoleEntity> UserRoles { get; set; }
  
    [ForeignKey(nameof(CreatedUserId))] 
    public UserEntity CreatedUser { get; set; }

    [ForeignKey(nameof(UpdatedUserId))] 
    public UserEntity UpdatedUser { get; set; }


    #endregion

}