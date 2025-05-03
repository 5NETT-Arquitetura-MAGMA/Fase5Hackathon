using Dapper.FastCrud;
using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.Migrator.Data.Entities;
using Microsoft.Data.SqlClient;

namespace HealthMed.CommandAPI.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IConfiguration _configuration;

        public DoctorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }

        public async Task<DoctorSchedule> CreateSchedule(DoctorSchedule schedule)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    await con.InsertAsync(schedule);
                }
                return schedule;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}