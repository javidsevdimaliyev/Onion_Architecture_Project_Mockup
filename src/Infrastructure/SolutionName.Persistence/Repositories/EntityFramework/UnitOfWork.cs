using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Persistence.Contexts;
using SolutionName.Domain.Entities.Common;

namespace SolutionName.Persistence.Repositories.EntityFramework
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EFDbContext _dbContext;
        public UnitOfWork(EFDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IReadRepository<TEntity, TKey> ReadRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
        {
            IReadRepository<TEntity, TKey> repo = new EFReadRepository<TEntity, TKey>(_dbContext as EFDbContext);

            return repo;
        }

        public IRepository<TEntity, TKey> Repository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
        {
            IRepository<TEntity, TKey> repo = new EFRepository<TEntity, TKey>(_dbContext as EFDbContext);
            return repo;
        }
        public System.Data.Common.DbConnection Connection()
        {
            return _dbContext.Database.GetDbConnection();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync()
        {          
            foreach (var entity in _dbContext.ChangeTracker.Entries())
            {
                await entity.ReloadAsync();
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
 
    }
}
