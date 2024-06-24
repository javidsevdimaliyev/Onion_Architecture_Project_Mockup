using Microsoft.AspNetCore.Identity;
using SolutionName.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Domain.Entities.Identity;

public class UserTokenEntity : IdentityUserToken<int>, IAuditableEntity<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }

    [Required] public bool IsDeleted { get; set; } = false;

    [Required] public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int CreatedUserId { get; set; }

    public DateTime UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public int OrderIndex { get; set; } = 0;

   
}