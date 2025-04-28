using HealthMed.Migrator.Data.Entities;

namespace HealthMed.CommandAPI.Interfaces.Repository
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(User user);
    }
}