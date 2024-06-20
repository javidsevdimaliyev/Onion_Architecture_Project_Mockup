using Dapper;
using Microsoft.Extensions.Configuration;
using SolutionName.Application.Abstractions.Repositories.Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolutionName.Application.Abstractions.Repositories.EntityFramework;
using System.Data.SqlClient;

namespace SolutionName.Persistence.Repositories.Dapper
{
    public class DapperRepository<TEntity> : IDapperRepository<TEntity>
    {
        private readonly IConfiguration config;
        private DbConnection connection;

        public DapperRepository(IUnitOfWork unitOfWork, IConfiguration config)
        {
            this.config = config;
            connection = unitOfWork.Connection();
        }

        private string Connectionstring = "FinDockConnection";

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(config.GetConnectionString(Connectionstring));
        }

        public void Dispose()
        {
            //GetDbconnection().Dispose();
            connection.Dispose();
        }

        public T Get<T>(string? sql)
        {
            //using IDbConnection db = new SqlConnection(config.GetConnectionString(Connectionstring));
            if (String.IsNullOrEmpty(sql))
                return default(T);

            return connection.QueryFirstOrDefault<T>(sql);
        }

        public async Task<T> GetAsync<T>(string? sql, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(sql))
                return default(T);
            // sql = $"Select * from [dbo].{typeof(TEntity).GetDisplayName()}";
            var command = new CommandDefinition(
                commandText: sql,
                commandType: CommandType.Text,
                commandTimeout: 120,
                cancellationToken: cancellationToken);
            return (await connection.QueryFirstOrDefaultAsync<T>(command)) ?? default(T);
        }

        public async Task<T> GetAsync<T>(string sql, DynamicParameters parms,
          CommandType commandType = CommandType.Text)
        {
            return await connection.QueryFirstAsync<T>(sql, parms, commandType: commandType);
        }


        public async Task<List<T>> GetAllAsync<T>(string sql, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(sql))
                return new List<T>();
            return (await connection.QueryAsync<T>(new CommandDefinition(
                commandText: sql,
                commandType: CommandType.Text,
                commandTimeout: 120,
                cancellationToken: cancellationToken))).ToList();
        }

      
        public async Task<List<T>> GetAllAsync<T>(string sql, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure)
        {
            return (await connection.QueryAsync<T>(sql, parms, commandType: commandType)).ToList();
        }

        public async Task<int> ExecuteScalarAsync(string? sql)
        {
            if (String.IsNullOrEmpty(sql))
                return 0;
            //sql = $"Select * from [dbo].{typeof(TEntity).GetDisplayName()} ";
            return await connection.ExecuteScalarAsync<int>(sql);
        }


        public async Task<int> ExecuteScalarAsync(string sql, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure)
        {
            return await connection.ExecuteScalarAsync<int>(sql, parms, commandType: commandType);
        }

        public async Task<T> Insert<T>(string sql, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using var tran = connection.BeginTransaction();
                try
                {
                    result = await connection.QueryFirstAsync<T>(sql, parms, commandType: commandType,
                        transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return result;
        }

        public async Task<T> Update<T>(string sql, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            //using IDbConnection db = new SqlConnection(config.GetConnectionString(Connectionstring));
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using var tran = connection.BeginTransaction();
                try
                {
                    result = (await connection.QueryFirstAsync<T>(sql, parms, commandType: commandType, transaction: tran));
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return result;
        }

        public async Task<int> UpdateAsync<T>(string sql, DynamicParameters parameters, CancellationToken cancellationToken = new())
        {
            try
            {
                var result = await connection.ExecuteAsync(new CommandDefinition(
                    commandText: sql,
                    parameters: parameters,
                    commandType: CommandType.Text,
                    commandTimeout: 120,
                    cancellationToken: cancellationToken));
                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }
    }
}
