using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SolutionName.Domain.Entities.Common;

namespace SolutionName.Domain.Entities.Identity;

public class UserClaimEntity : IdentityUserClaim<int>, IEntity<int>
{
    public int ClaimId { get; set; }

    [ForeignKey(nameof(ClaimId))] 
    public ClaimEntity Claim { get; set; }
    public bool IsDeleted { get; set; }
}