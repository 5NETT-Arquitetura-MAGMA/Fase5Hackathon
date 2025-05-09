using Dapper;
using Dapper.FastCrud;
using HealthMed.CommandAPI.Interfaces.Repository;
using HealthMed.Migrator.Data.Entities;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

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

        public async Task<User> GetDoctorByCRM(string crm)
        {
            try
            {
                var user = new User();
                crm = CleanString(crm);
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var query = $@"SELECT Id, Name, PhoneNumber, EmailAddress, [Login], Password, [Type], CreationTime, UpdateTime, Specialty, SecurityHash, DoctorConsultationStatusId, PatientConsultationStatusId
                                    FROM Users where dbo.CleanLogin(Login) = '{crm}'";
                    user = await con.QueryFirstOrDefaultAsync<User>(query);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUserByCPF(string cpf)
        {
            try
            {
                var user = new User();
                cpf = CleanString(cpf);
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var query = $@"SELECT Id, Name, PhoneNumber, EmailAddress, [Login], Password, [Type], CreationTime, UpdateTime, Specialty, SecurityHash, DoctorConsultationStatusId, PatientConsultationStatusId
                                    FROM Users where dbo.CleanLogin(Login) = '{cpf}'";
                    user = await con.QueryFirstOrDefaultAsync<User>(query);
                }
                return user;
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
                var user = new User();
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var query = $@"SELECT Id, Name, PhoneNumber, EmailAddress, [Login], Password, [Type], CreationTime, UpdateTime, Specialty, SecurityHash, DoctorConsultationStatusId, PatientConsultationStatusId
                                    FROM Users where Login = '{login}'";
                    user = await con.QueryFirstOrDefaultAsync<User>(query);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> Get(Guid id)
        {
            try
            {
                var user = new User();
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                await using (var con = new SqlConnection(connectionString))
                {
                    var query = $@"SELECT Id, Name, PhoneNumber, EmailAddress, [Login], Password, [Type], CreationTime, UpdateTime, Specialty, SecurityHash, DoctorConsultationStatusId, PatientConsultationStatusId
                                    FROM Users where id = '{id}'";
                    user = await con.QueryFirstOrDefaultAsync<User>(query);
                }
                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
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

        #region Private Methods

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            // Remove espaços
            input = input.Replace(" ", "");
            // Remove caracteres especiais (mantém apenas letras e números)
            input = Regex.Replace(input, "[^a-zA-Z0-9]", "");
            return input;
        }

        #endregion Private Methods
    }
}