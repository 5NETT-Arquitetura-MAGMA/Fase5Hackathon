using HealthMed.Migrator.Data.Entities;

namespace HealthMed.CommandAPI.Interfaces.Repository
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(User user);

        public Task<User> Get(Guid id);

        public Task<User> GetDoctorByCRM(string crm);

        public Task<User> GetUserByCPF(string cpf);
    }
}