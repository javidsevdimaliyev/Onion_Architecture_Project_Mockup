using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
    }

    public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public virtual TKey Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
