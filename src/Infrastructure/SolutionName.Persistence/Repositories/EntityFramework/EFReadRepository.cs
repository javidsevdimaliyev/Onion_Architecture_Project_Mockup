using Microsoft.EntityFrameworkCore;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using SolutionName.Domain.Entities.Common;
using SolutionName.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SolutionName.Persistence.Repositories.EntityFramework
{
    public class EFReadRepository<TEntity, TKey> : IReadRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {

        private readonly EFDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        readonly DbSet<TEntity> _dbSet;

        public DbSet<TEntity> Table => throw new NotImplementedException();

        public EFReadRepository(EFDbContext context)
        {
            _context = context;
            _unitOfWork = new UnitOfWork(context as EFDbContext);
            _dbSet = _context.Set<TEntity>();
        }

        public ValueTask<TEntity?> FindAsync(object id, CancellationToken cancellationToken = default)
        {
            return _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<TEntity> FindByIdAsync(object id, CancellationToken cancellationToken = default, bool tracking = true)
        {
            var query = _dbSet.IgnoreQueryFilters();
            return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<bool> FindAnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await FindQueryable().AnyAsync(predicate);
        }

        public async Task<TEntity> FindFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool tracking = true)
        {
            return await FindQueryable(predicate, tracking).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, bool tracking = true)
        {
            return await FindQueryable(predicate, tracking).ToListAsync(cancellationToken);
        }

        public async Task<ICollection<TEntity>> FindAllAsync(CancellationToken cancellationToken = default, bool tracking = true)
        {
            return await FindQueryable(null, tracking).ToListAsync(cancellationToken: cancellationToken);
        }


        public async Task<ICollection<TType>> FindSpecificColumnsAsync<TType>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TType>> select) where TType : class
        {
            IQueryable<TEntity> query = FindQueryable(predicate);

            return await query.Select(select).ToListAsync();
        }

        public IQueryable<TEntity> Query(bool tracking = true)
        {
            var type = typeof(TEntity);
            var query = tracking
                ? _dbSet.AsQueryable()
                : _dbSet.AsQueryable().AsNoTracking();
            return query.TagWith(type.FullName!);
        }

        public IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate = null, bool tracking = true, bool splitQuery = false)
        {
            IQueryable<TEntity> query = Query(tracking);
            if (predicate != null && predicate.ToString().Contains(".IsDeleted"))
                query = query.IgnoreQueryFilters();

            var type = typeof(TEntity);
            if (predicate != null)
                query = query.Where(predicate);

            if (splitQuery)
                query = query.AsSplitQuery();

            return query;
        }

        public IQueryable<TEntity> FindQueryable(Expression<Func<TEntity, bool>> predicate, bool tracking = true, bool splitQuery = false,
            params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            IQueryable<TEntity> query = FindQueryable(predicate, tracking, splitQuery);
            if (includeExpressions.Length > 0)
            {
                query = includeExpressions.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                //foreach (var includeExpression in includeExpressions)
                //   query = query.Include(includeExpression);               
            }

            return query;
        }


        public IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page, int pageSize, out int count)
        {
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            count = query.Count();
            query = query.Skip((page) * pageSize).Take(pageSize);
            return query;
        }

        public System.Data.DataTable SelectSqlRaw(string sqlQuery)
        {
            DataTable dataTable = new DataTable();

            var connection = _context.Database.GetDbConnection();

            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);

            using (var cmd = dbFactory.CreateCommand())
            {
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;

                    using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;

                        if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();

                        adapter.Fill(dataTable);

                        cmd.Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    if (cmd.Connection.State != ConnectionState.Closed) cmd.Connection.Close();

                    throw ex;
                }

            }

            return dataTable;
        }
    }
}
