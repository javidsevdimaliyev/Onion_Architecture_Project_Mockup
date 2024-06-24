namespace SolutionName.Domain.Entities.Common
{
    public interface IEntity
    {
        bool IsDeleted { get; set; }
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
