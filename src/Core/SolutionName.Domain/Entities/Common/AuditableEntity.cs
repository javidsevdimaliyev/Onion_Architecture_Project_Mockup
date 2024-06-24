using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionName.Domain.Entities.Common
{
    public abstract class AuditableEntity<TKey> : BaseEntity<TKey>, IAuditableEntity<TKey>
    {
     
        [Required]
        [Column("CreateDate")]
        public DateTime CreatedDate { get; set; }

        [Column("UpdateDate")]
        public DateTime UpdatedDate { get; set; }

        [Column("CreatedUserId")]
        public long CreatedUserId { get; set; }

        [Column("UpdatedUserId")]
        public long? UpdatedUserId { get; set; }

        [Required]
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }

        [Column("OrderIndex")]
        public int OrderIndex { get; set; } = 0;

        //[ForeignKey(nameof(CreateUserId))]
        //public UserEntity CreateUser { get; set; }

    }
}
