using SolutionName.Domain.Entities.Common;
using System.Data.Common;

namespace SolutionName.Application.Abstractions.Repositories.EntityFramework
{
    public interface IUnitOfWork
    {
        IRepository<TEntity, TKey> Repository<TEntity, TKey>() 
            where TEntity : class, IEntity<TKey>;
        IReadRepository<TEntity, TKey> ReadRepository<TEntity, TKey>() 
            where TEntity : class, IEntity<TKey>;

        DbConnection Connection();    
        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync();
    }
}
