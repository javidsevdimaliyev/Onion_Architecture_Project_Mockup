using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SolutionName.Application.Abstractions.Repositories.Dapper
{
    public interface IDapperRepository<TEntity>
    {
        DbConnection GetDbconnection();

        T Get<T>(string? sql);
        Task<T> GetAsync<T>(string sql, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync<T>(string sql, CancellationToken cancellationToken = new());
        Task<int> ExecuteScalarAsync(string sql);

        Task<T> GetAsync<T>(string sql, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        Task<List<T>> GetAllAsync<T>(string sql, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);

        Task<int> ExecuteScalarAsync(string sql, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure);

        Task<T> Insert<T>(string sql, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Update<T>(string sql, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> UpdateAsync<T>(string sql, DynamicParameters parameters, CancellationToken cancellationToken = new());
    }
}
