namespace SolutionName.Domain.Entities.Common
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        DateTime UpdatedDate { get; set; }

        long CreatedUserId { get; set; }

        long? UpdatedUserId { get; set; }

        int OrderIndex { get; set; }
    }

    public interface IAuditableEntity<TKey> : IAuditableEntity, IEntity<TKey>
    {
    }

}
