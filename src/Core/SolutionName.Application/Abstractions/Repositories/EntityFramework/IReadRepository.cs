using SolutionName.Domain.Entities.Common;
using System.Data;
using System.Linq.Expressions;

namespace SolutionName.Application.Abstractions.Repositories.EntityFramework
{
    public interface IReadRepository<TEntity,TKey> where TEntity : class, IEntity<TKey>
    {

        ValueTask<TEntity> FindAsync(object id, CancellationToken cancellationToken = default);
        Task<ICollection<TEntity>> FindAllAsync(CancellationToken cancellationToken = default, bool tracking = true);

        Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default, bool tracking = true);

        Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool tracking = true);

        Task<bool> FindAnyAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool tracking = true);

        Task<ICollection<TType>> FindSpecificColumnsAsync<TType>(Expression<Func<TEntity, bool>> predicate,
           Expression<Func<TEntity, TType>> select) where TType : class;


        IQueryable<TEntity> Query(bool tracking = true);

        IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true,
            bool splitQuery = false);

        IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate, bool tracking = true,
            params Expression<Func<TEntity, object>>[] includeExpressions);

        IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page, int pageSize, out int count);


        DataTable SelectSqlRaw(string sqlQuery);
    }
}
