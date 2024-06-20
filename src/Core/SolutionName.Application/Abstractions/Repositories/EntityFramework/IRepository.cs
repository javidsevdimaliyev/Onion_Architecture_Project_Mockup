using SolutionName.Domain.Entities.Common;
using System.Linq.Expressions;

namespace SolutionName.Application.Abstractions.Repositories.EntityFramework
{
    public interface IRepository<TEntity,TKey> : IReadRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        Task AddAsync(TEntity entity, bool isCommit = false,
       CancellationToken cancellationToken = default);

        Task AddRangeAsync(ICollection<TEntity> entities, bool isCommit = false,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(TEntity updated, bool isCommit = false,
            CancellationToken cancellationToken = default);

        Task UpdateRangeAsync(ICollection<TEntity> entitie, bool isCommit = false,
            CancellationToken cancellationToken = default);

        Task UpdateSpecificPropertiesAsync(TEntity updated, bool isCommit = false, bool isExceptThisProperties=false,
            params Expression<Func<TEntity, object>>[] properties);

        Task DeleteAsync(TEntity entity, bool isCommit = false,
            CancellationToken cancellationToken = default);

        Task DeleteRangeAsync(ICollection<TEntity> entities, bool isCommit = false,
            CancellationToken cancellationToken = default);

        Task<bool> SoftDeleteAsync(object id, CancellationToken cancellationToken = default);

        Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        //Task BulkInsertAsync(IList<TEntity> entities, bool hasChild = true);
        //Task BulkUpdateAsync(IList<TEntity> entities, bool hasChild = true);

        ValueTask<int> ExecuteSqlRawAsync(string sql,
            CancellationToken cancellationToken = default);
    }
}
