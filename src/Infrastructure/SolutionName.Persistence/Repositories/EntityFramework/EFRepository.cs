using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Domain.Entities.Common;
using SolutionName.Persistence.Contexts;
using System.Linq.Expressions;

namespace SolutionName.Persistence.Repositories.EntityFramework
{
    public class EFRepository<TEntity, TKey> : EFReadRepository<TEntity, TKey> , IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        readonly private EFDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        DbSet<TEntity> _dbSet;

        public EFRepository(EFDbContext context) : base(context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(context);
            _dbSet = _context.Set<TEntity>();
        }

        #region .:: Helpers ::. 
        public void Attach(TEntity entity) => _dbSet.Attach(entity);

        public void AttachRange(IEnumerable<TEntity> entities) => _dbSet.AttachRange(entities);

        private void Detach(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        private void DetachRange(ICollection<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Detach(entity);
            }
        }

        private void UnchangedProperty(TEntity entity)
        {
            if (entity is IAuditableEntity<TKey>)
            {
                _context.Entry(entity).Property("CreatedDate").IsModified = false;
                _context.Entry(entity).Property("CreatedUserId").IsModified = false;

                //_context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                //_context.Entry(entity).Property(x => x.CreatedUserId).IsModified = false;
            }
        }

        public async ValueTask<int> ExecuteSqlRawAsync(string sql,
           CancellationToken cancellationToken = default)
        {
            return await _context
                .Database
                .ExecuteSqlRawAsync(sql, cancellationToken);
        }


        #endregion

        #region .:: Add operations ::. 
        public async Task AddAsync(TEntity entity, bool isCommit = false, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            if (isCommit)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                Detach(entity);
            }
        }

        public async Task AddRangeAsync(ICollection<TEntity> entities, bool isCommit = false, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);

            if (isCommit)
            {
                await _unitOfWork.CommitAsync(cancellationToken);

                DetachRange(entities);
            }
        }

        //public async Task BulkInsertAsync(IList<TEntity> entities, bool hasChild = true)
        //{
        //    await _context.BulkInsertAsync(entities, options => options.IncludeGraph = hasChild);
        //}

        //public async Task BulkUpdateAsync(IList<TEntity> entities, bool hasChild = true)
        //{
        //    await _context.BulkUpdateAsync(entities, options => options.IncludeGraph = hasChild);
        //}

       
        #endregion

        #region .:: Update operations ::. 

        public async Task UpdateAsync(TEntity entity, bool isCommit = false, CancellationToken cancellationToken = default)
        {
            _dbSet.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;

            UnchangedProperty(entity);

            if (isCommit)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
                Detach(entity);
            }
        }

        public async Task UpdateRangeAsync(ICollection<TEntity> entities, bool isCommit = false, CancellationToken cancellationToken = default)
        {
            foreach (var updated in entities)
            {
                _dbSet.Attach(updated);

                _context.Entry(updated).State = EntityState.Modified;

                UnchangedProperty(updated);
            }

            if (isCommit)
            {
                await _unitOfWork.CommitAsync(cancellationToken);

                DetachRange(entities);
            }
        }

        public async Task UpdateSpecificPropertiesAsync(TEntity entity, bool isCommit = false, bool isExceptThisProperties = false, params Expression<Func<TEntity, object>>[] properties)
        {
            var entry = _context.Entry(entity);
            if (!isExceptThisProperties)
            {
                entry.State = EntityState.Unchanged;

                foreach (var prop in properties)
                {
                    entry.Property(prop).IsModified = true;
                }
            }
            else
            {
                entry.State = EntityState.Modified;

                foreach (var prop in properties)
                {
                    entry.Property(prop).IsModified = false;
                }
            }

            if (isCommit)
            {
                await _unitOfWork.CommitAsync();

                Detach(entity);
            }
        }

     
        #endregion

        #region .:: Delete operations ::. 

        public async Task DeleteAsync(TEntity entity, bool isCommit = false, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);

            if (isCommit)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
        }

        public async Task DeleteRangeAsync(ICollection<TEntity> entities, bool isCommit = false, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(entities);

            if (isCommit)
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
        }

        public async Task<bool> SoftDeleteAsync(object id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) throw new ArgumentNullException($"On Delete {typeof(TEntity).FullName} is null.");
            if (entity is IAuditableEntity<TKey>)
            {
                _dbSet.Attach(entity);
                entity.IsDeleted = true;
                _context.Entry(entity).State = EntityState.Modified;
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            return false;
        }

        public async Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
            {
                var entities = _dbSet.Where(predicate).ToList();
                foreach (var entity in entities)
                {
                    _dbSet.Attach(entity);
                    entity.IsDeleted = true;
                    _context.Entry(entity).State = EntityState.Modified;
                }

                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }

            return false;
        }
        #endregion
    }
}
