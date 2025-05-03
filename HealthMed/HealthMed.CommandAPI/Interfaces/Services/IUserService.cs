using HealthMed.Migrator.Data.Entities;

namespace HealthMed.CommandAPI.Interfaces.Services
{
    public interface IUserService
    {
        public Task CreateUser(string name, string phoneNumber, string emailAddress, string login, string password, string? specialty);

        public Task<User> Get(Guid id);

        public Task<User> GetDoctorByCRM(string crm);

        public Task<User> GetUserByCPF(string cpf);
    }
}