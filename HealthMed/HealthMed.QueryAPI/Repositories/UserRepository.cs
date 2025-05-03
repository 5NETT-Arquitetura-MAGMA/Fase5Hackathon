using Dapper;
using Dapper.FastCrud;
using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
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
                    var users = await con.QueryAsync<User>(sql, parameters);

                    var countSql = $"SELECT COUNT(*) FROM Users {whereClause}";
                    var totalCount = await con.ExecuteScalarAsync<int>(countSql, parameters);

                    return (users.ToList(), totalCount);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}