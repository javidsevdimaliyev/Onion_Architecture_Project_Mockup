using SolutionName.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Domain.Entities.Identity;

[Table("Claims")]
public class ClaimEntity : BaseEntity<int>
{
    public ClaimEntity()
    {
       
    }
    public ClaimEntity(string claimName) : this()
    {
        Name = claimName;
    }

    public int? ParentId { get; set; }

    [MaxLength(255)] public string Name { get; set; }

    [MaxLength(255)] public string? Module { get; set; }


    [MaxLength(255)] public string? Page { get; set; }

    public int OrderIndex { get; set; } = 0;

    public ICollection<RoleClaimEntity> RoleClaims { get; set; }
}