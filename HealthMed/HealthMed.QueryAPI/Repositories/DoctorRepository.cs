using Dapper;
using Dapper.FastCrud;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using HealthMed.QueryAPI.Interfaces.Repositories;
using Microsoft.Data.SqlClient;

namespace HealthMed.QueryAPI.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IConfiguration _configuration;

        public DoctorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;
        }

        public async Task<(List<DoctorOffDays>, int)> GetOffDays(Guid id, int pageSize, int pageNumber, string sortBy = "CreationTime", string sortDirection = "asc")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var whereClauses = new List<string>();
                    var parameters = new DynamicParameters();

                    whereClauses.Add("DoctorId = @DoctorId");
                    parameters.Add("DoctorId", id);

                    var whereClause = whereClauses.Count > 0 ? $"WHERE {string.Join(" AND ", whereClauses)}" : "";

                    string orderByClause = "ORDER BY CreationTime";

                    var allowedColumns = new List<string> { "Id", "CreationTime" };

                    if (!string.IsNullOrEmpty(sortBy) && allowedColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
                    {
                        orderByClause = $"ORDER BY {sortBy} {(sortDirection?.ToLower() == "desc" ? "DESC" : "ASC")}";
                    }

                    var offset = (pageNumber - 1) * pageSize;
                    var paginationClause = $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var sql = $"SELECT * FROM DoctorOffDays {whereClause} {orderByClause} {paginationClause}";
                    var res = await con.QueryAsync<DoctorOffDays>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM DoctorOffDays {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (res.ToList(), totalCount);
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

        public async Task<(List<User>, int)> GetAllDoctors(Guid? doctorId, int pageSize, int pageNumber, string sortBy = "CreationTime", string sortDirection = "asc")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var whereClauses = new List<string>();
                    var parameters = new DynamicParameters();

                    if (doctorId.HasValue)
                    {
                        whereClauses.Add("DoctorId = @DoctorId");
                        parameters.Add("DoctorId", doctorId.Value);
                    }
                    whereClauses.Add("Type = @Type");
                    parameters.Add("Type", UserType.Doctor);

                    var whereClause = whereClauses.Count > 0 ? $"WHERE {string.Join(" AND ", whereClauses)}" : "";

                    string orderByClause = "ORDER BY CreationTime";

                    var allowedColumns = new List<string> { "Name", "EmailAddress", "Id", "CreationTime" };

                    if (!string.IsNullOrEmpty(sortBy) && allowedColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
                    {
                        orderByClause = $"ORDER BY {sortBy} {(sortDirection?.ToLower() == "desc" ? "DESC" : "ASC")}";
                    }

                    var offset = (pageNumber - 1) * pageSize;
                    var paginationClause = $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var sql = $"SELECT * FROM Users {whereClause} {orderByClause} {paginationClause}";
                    var res = await con.QueryAsync<User>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM Users {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (res.ToList(), totalCount);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<DoctorSchedule>, int)> GetWorkDays(Guid id, int pageSize, int pageNumber, string sortBy = "CreationTime", string sortDirection = "asc")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var whereClauses = new List<string>();
                    var parameters = new DynamicParameters();

                    whereClauses.Add("DoctorId = @DoctorId");
                    parameters.Add("DoctorId", id);

                    var whereClause = whereClauses.Count > 0 ? $"WHERE {string.Join(" AND ", whereClauses)}" : "";

                    string orderByClause = "ORDER BY CreationTime";

                    var allowedColumns = new List<string> { "Id", "CreationTime" };

                    if (!string.IsNullOrEmpty(sortBy) && allowedColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
                    {
                        orderByClause = $"ORDER BY {sortBy} {(sortDirection?.ToLower() == "desc" ? "DESC" : "ASC")}";
                    }

                    var offset = (pageNumber - 1) * pageSize;
                    var paginationClause = $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var sql = $"SELECT * FROM DoctorSchedules {whereClause} {orderByClause} {paginationClause}";
                    var res = await con.QueryAsync<DoctorSchedule>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM DoctorSchedules {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (res.ToList(), totalCount);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<MedicalConsultation>, int)> ListMedicalConsultation(Guid doctorId, int pageSize, int pageNumber, string sortBy = "CreationTime", string sortDirection = "asc")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var whereClauses = new List<string>();
                    var parameters = new DynamicParameters();

                    whereClauses.Add("DoctorId = @DoctorId");
                    parameters.Add("DoctorId", doctorId);

                    var whereClause = whereClauses.Count > 0 ? $"WHERE {string.Join(" AND ", whereClauses)}" : "";

                    string orderByClause = "ORDER BY CreationTime";

                    var allowedColumns = new List<string> { "Id", "CreationTime" };

                    if (!string.IsNullOrEmpty(sortBy) && allowedColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
                    {
                        orderByClause = $"ORDER BY {sortBy} {(sortDirection?.ToLower() == "desc" ? "DESC" : "ASC")}";
                    }

                    var offset = (pageNumber - 1) * pageSize;
                    var paginationClause = $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var sql = $"SELECT * FROM MedicalConsultations {whereClause} {orderByClause} {paginationClause}";
                    var res = await con.QueryAsync<MedicalConsultation>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM MedicalConsultations {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (res.ToList(), totalCount);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<MedicalConsultation>, int)> ListPendingMedicalConsultation(Guid doctorId, int pageSize, int pageNumber, string sortBy = "CreationTime", string sortDirection = "asc")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var whereClauses = new List<string>();
                    var parameters = new DynamicParameters();

                    whereClauses.Add("DoctorId = @DoctorId");
                    parameters.Add("DoctorId", doctorId);

                    whereClauses.Add("Status = @Status");
                    parameters.Add("Status", (int)ConsultationStatus.PendingConfirmation);

                    var whereClause = whereClauses.Count > 0 ? $"WHERE {string.Join(" AND ", whereClauses)}" : "";

                    string orderByClause = "ORDER BY CreationTime";

                    var allowedColumns = new List<string> { "Id", "CreationTime" };

                    if (!string.IsNullOrEmpty(sortBy) && allowedColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
                    {
                        orderByClause = $"ORDER BY {sortBy} {(sortDirection?.ToLower() == "desc" ? "DESC" : "ASC")}";
                    }

                    var offset = (pageNumber - 1) * pageSize;
                    var paginationClause = $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var sql = $"SELECT * FROM MedicalConsultations {whereClause} {orderByClause} {paginationClause}";
                    var res = await con.QueryAsync<MedicalConsultation>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM MedicalConsultations {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (res.ToList(), totalCount);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<(List<MedicalConsultation>, int)> ListPatientMedicalConsultation(Guid patientId, int pageSize, int pageNumber, string sortBy = "CreationTime", string sortDirection = "desc")
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var whereClauses = new List<string>();
                    var parameters = new DynamicParameters();

                    whereClauses.Add("PatientId = @PatientId");
                    parameters.Add("PatientId", patientId);

                    var whereClause = whereClauses.Count > 0 ? $"WHERE {string.Join(" AND ", whereClauses)}" : "";

                    string orderByClause = "ORDER BY CreationTime";

                    var allowedColumns = new List<string> { "Id", "CreationTime" };

                    if (!string.IsNullOrEmpty(sortBy) && allowedColumns.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
                    {
                        orderByClause = $"ORDER BY {sortBy} {(sortDirection?.ToLower() == "desc" ? "DESC" : "ASC")}";
                    }

                    var offset = (pageNumber - 1) * pageSize;
                    var paginationClause = $"OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                    var sql = $"SELECT * FROM MedicalConsultations {whereClause} {orderByClause} {paginationClause}";
                    var res = await con.QueryAsync<MedicalConsultation>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM MedicalConsultations {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (res.ToList(), totalCount);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}