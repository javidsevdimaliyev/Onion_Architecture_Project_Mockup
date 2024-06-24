using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SolutionName.Domain.Entities.Common;

namespace SolutionName.Domain.Entities.Identity;

public class RoleClaimEntity : IdentityRoleClaim<int>, IAuditableEntity<int>
{
    public RoleClaimEntity()
    {
    }

    public RoleClaimEntity(int claimId, string claimType) : this()
    {
        ClaimId = claimId;
        ClaimType = claimType;
    }

    public RoleClaimEntity(string claimType) : this()
    {
        ClaimType = claimType;
    }

    //public long Id { get; set; }
    public int ClaimId { get; set; }
    public int RoleId { get; set; }

    public ClaimEntity Claim { get; set; }

    public RoleEntity Role { get; set; }

    public DateTime CreatedDate { get; set; }
    public int CreatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int? UpdatedUserId { get; set; }
    public bool IsDeleted { get; set; }
    public int OrderIndex { get; set; }

    public void SetClaim(string claimName)
    {
        Claim = new ClaimEntity(claimName);
    }
}