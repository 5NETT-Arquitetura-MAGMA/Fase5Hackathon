using Dapper;
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

        public async Task<MedicalConsultation> GetConsultation(Guid id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var sql = $"SELECT * FROM MedicalConsultations where id = '{id}'";
                    return await con.QueryFirstOrDefaultAsync<MedicalConsultation>(sql);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MedicalConsultation>> GetConsultations(Guid doctorId)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var sql = $"SELECT * FROM MedicalConsultations where DoctorId = '{doctorId}'";
                    return (await con.QueryAsync<MedicalConsultation>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DoctorOffDays>> GetOffDays(Guid id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var sql = $"SELECT * FROM DoctorOffDays where DoctorId = '{id}'";
                    return (await con.QueryAsync<DoctorOffDays>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DoctorSchedule>> GetWorkDays(Guid id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var sql = $"SELECT * FROM DoctorSchedules where DoctorId = '{id}'";
                    return (await con.QueryAsync<DoctorSchedule>(sql)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MedicalConsultation> CreateConsultation(MedicalConsultation consultation)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    await con.InsertAsync(consultation);
                }
                return consultation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<MedicalConsultation> UpdateConsultation(MedicalConsultation consultation)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    await con.UpdateAsync(consultation);
                }
                return consultation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DoctorOffDays> CreateOffDay(DoctorOffDays offDay)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    await con.InsertAsync(offDay);
                }
                return offDay;
            }
            catch (Exception ex)
            {
                throw;
            }
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