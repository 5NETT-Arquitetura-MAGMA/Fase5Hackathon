using Dapper;
using Dapper.FastCrud;
using HealthMed.Migrator.Data.Entities;
using HealthMed.QueryAPI.Interfaces.Repositories;
using Microsoft.Data.SqlClient;

namespace HealthMed.QueryAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }

        public async Task<User> Get(Guid id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var sql = $"SELECT * FROM Users where id = '{id}'";
                    return await con.QueryFirstOrDefaultAsync<User>(sql);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> Get(string login)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var sql = $"SELECT * FROM Users where Login = '{login}'";
                    return await con.QueryFirstOrDefaultAsync<User>(sql);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}