using Dapper.FastCrud;
using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.Migrator.Data.Entities;
using Microsoft.Data.SqlClient;

namespace HealthMed.CommandAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    await con.InsertAsync(user);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}