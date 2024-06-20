using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SolutionName.Domain.Entities.Common;

namespace SolutionName.Domain.Entities.Identity;

public class RoleEntity : IdentityRole<int>, IAuditableEntity<int>
{
    private readonly List<RoleClaimEntity> _RoleClaims;
    public RoleEntity()
    {
        _RoleClaims = new List<RoleClaimEntity>();      
    }

    public RoleEntity(string roleName) : this()
    {
        Name = roleName;
        NormalizedName = roleName.ToUpper();
      
    }

    public IReadOnlyCollection<RoleClaimEntity> RoleClaims => _RoleClaims;
   
    //[Key] public override long Id { get; set; }

    public DateTime CreatedDate { get; set; }
    public long CreatedUserId { get; set; }
    public DateTime UpdatedDate { get; set; }
    public long? UpdatedUserId { get; set; }
    public bool IsDeleted { get; set; }
    public int OrderIndex { get; set; }

    #region METHODS

    #region claims

    /// <summary>
    ///     for static claims
    /// </summary>
    /// <param name="claimId"></param>
    /// <param name="claimType"></param>
    public void AddClaims(int claimId, string claimType)
    {
        _RoleClaims.Add(new RoleClaimEntity(claimId, claimType));
    }

    /// <summary>
    ///     for dynamic claims
    /// </summary>
    /// <param name="claimName"></param>
    /// <param name="claimValue"></param>
    public void AddRoleClaim(string claimName, string claimValue)
    {
        var newRoleClaim = new RoleClaimEntity(claimName);
        newRoleClaim.SetClaim(claimName);
        _RoleClaims.Add(newRoleClaim);
    }

    public void SetName(string name)
    {
        Name = name;
        NormalizedName = name.ToUpper();
    }

    public void SetFunctionalRoleClaimValue(string claimValue)
    {
        var roleClaim = RoleClaims.FirstOrDefault();
        if (roleClaim != null) roleClaim.Claim.Name = claimValue;
    }

    public void ClearClaims()
    {
        _RoleClaims.Clear();
    }

    #endregion

    #endregion
}